using Qexpeditive.BusinessLayer.Messages.Dossiers;
using Qexpeditive.Web.BackendActions;
using Qframework.Common.Exceptions;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Threading;

public partial class RegenerateCover_RegenerateCover : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
  
    protected void SubmitRegenerate_Click(object sender, EventArgs e)
    {
        string retAlert = ValidateExpNumber();
        if (retAlert != "OK")
        {
            alertSucces.Style.Add("display","none");
            alertError.InnerHtml = retAlert;
            alertError.Style.Add("display", "block");
        }
        else
        {
            alertError.Style.Add("display", "none");
            alertSucces.Style.Add("display", "block");
        }
        nroExpediente.Text = "";
    }
    
    private string ValidateExpNumber()
    {
        string msg = "OK";
        var nroExpedienteFormato = nroExpediente.Text.Split('/');
        if (nroExpedienteFormato.Count() == 2)
        {
            int aux;
            var nroExp = nroExpediente.Text.Split('/')[0];
            var anioExp = nroExpediente.Text.Split('/')[1];
            if (Int32.TryParse(nroExp, out aux) && Int32.TryParse(anioExp, out aux))
            {
                string dossierID = "";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["QExpeditive"].ToString()))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT DossierID " +
                                                                "FROM Dossier " +
                                                                "WHERE DossierGlobalRelativeID = @nroExp and YEAR(StartDate) = @anioExp;"
                                                                , con))
                    {
                        command.Parameters.Add("@nroExp", System.Data.SqlDbType.VarChar);
                        command.Parameters["@nroExp"].Value = nroExp;
                        command.Parameters.Add("@anioExp", System.Data.SqlDbType.Int);
                        command.Parameters["@anioExp"].Value = anioExp;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            if (reader.HasRows)
                            {
                                dossierID = reader.GetGuid(0).ToString();
                                if (!VerifyQueued(dossierID))
                                {
                                    this.RegenerateCover(dossierID);
                                }
                                else
                                {
                                    msg = "La carátula ya se encuentra en el proceso de regeneración.";
                                }
                            }
                            else
                            {
                                msg = "El trámite que ha ingresado no existe.";
                            }
                        }
                    }
                    con.Close();
                }
            }
            else
            {
                msg = "El trámite ingresado no tiene un formato correcto (Ej. 0001/2017)";
            }
        }
        else 
        {
            msg = "El trámite ingresado no tiene un formato correcto (Ej. 0001/2017)";
        }
        return msg;
    }

    
    private void RegenerateCover(string dossierID)
    {
        using (SqlConnection con  = new SqlConnection(ConfigurationManager.ConnectionStrings["QExpeditive"].ToString()))
        {
            var query = "EXEC RegeneratePDF @DossierId=@DossierId2, @ActuationId=NULL";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                con.Open();
                command.Parameters.AddWithValue("@DossierId2", SqlDbType.UniqueIdentifier).Value = dossierID;
                command.ExecuteNonQuery();
                con.Close();
                this.WaitRegenerate(dossierID);
            }
        }
    }

    private void WaitRegenerate(string dossierID)
    {
        bool validate = false;
        using (SqlConnection con  = new SqlConnection( ConfigurationManager.ConnectionStrings["QExpeditive"].ToString()))
        {
            con.Open();
            using (SqlCommand command = new SqlCommand("SELECT [DossierID]"+
                                                        "FROM [CoverToPrint]"+
                                                        "WHERE DossierID = @DossierId",con))
            {
                command.Parameters.AddWithValue("@DossierId", SqlDbType.UniqueIdentifier);
                command.Parameters["@DossierId"].Value = dossierID;
                while (validate == false)
                {
                    Thread.Sleep(5000);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            validate = true;
                        }
                    }
                }
            }
            con.Close();
        }
    }

    private bool VerifyQueued(string dossierID) //Verifica que no exista una caratula regenerandose.
    {
        var verify = false;
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["QExpeditive"].ToString()))
        {
            con.Open();
            using (SqlCommand command = new SqlCommand("SELECT [DossierID]" +
                                                        "FROM [CoverToPrint]" +
                                                        "WHERE DossierID = @DossierId", con))
            {
                command.Parameters.AddWithValue("@DossierId", SqlDbType.UniqueIdentifier);
                command.Parameters["@DossierId"].Value = dossierID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        verify = true;
                    }
                    else
                    {
                        verify = false;
                    }
                }
            }
            con.Close();
        }
        return verify;
    }
}
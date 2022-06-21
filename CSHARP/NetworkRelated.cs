using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Collections.Specialized;
using System.IO;

//A method to send e-mail
public Tuple<bool, string> SendMail(string smtpServer, string to, string from, string fromName, 
    int portNumber, string subject, string body, string userName, string password, 
    bool enableSsl, bool isBodyHtml)
{
    try
    {
        SmtpClient server = new SmtpClient(smtpServer); //"mail.example.com"
        server.EnableSsl = enableSsl;
        server.Port = portNumber;
        server.Credentials = new System.Net.NetworkCredential(userName, password);
        MailMessage email = new MailMessage();
        email.IsBodyHtml = true;
        //email.From = new MailAddress(from);
        email.From = new MailAddress(from, fromName);
        email.Subject = subject;
        email.Body = body;
        email.IsBodyHtml = isBodyHtml;
        //adress: person1@example.com;person2@example.com;person3@example.com
        foreach (var address in to.Split(';'))
        {
            email.To.Add(new MailAddress(address.Trim(), ""));
        }
        server.Send(email);

        return Tuple.Create(true, "Success");

    }
    catch (Exception ex)
    {
        return Tuple.Create(false, "Fail: " + ex.ToString());
    }
}

//An alternative method to send e-mail
public Tuple<bool, string> SendMail(string smtpServer, string to, string toName, string from, 
    string fromName, int portNumber, string mailsubject, string mailbody, string password, 
    bool enableSsl, bool isBodyHtml)
{
    try
    {
        var fromAddress = new MailAddress(from, fromName);
        var toAddress = new MailAddress(to, toName);
        string fromPassword = password;
        string subject = mailsubject;
        string body = mailbody;

        var smtp = new SmtpClient
        {
            Host = smtpServer,
            Port = portNumber,
            EnableSsl = enableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };
        using (var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = isBodyHtml,
        })
        {
            smtp.Send(message);
        }

        return Tuple.Create(true, "Success");

    }
    catch (Exception ex)
    {
        return Tuple.Create(false, "Fail: " + ex.ToString());
    }
}

//Another alternative method to send e-mail
public Tuple<bool, string> SendMail(int port, string host, string fromEmail, string toEmail, 
    string password, string subject, string body, bool ssl = true, int timeout = 10000)
{
    try
    {
        SmtpClient client = new SmtpClient();
        client.Port = port; //587
        client.Host = host; //"smtp.gmail.com"
        client.EnableSsl = ssl; //default true
        client.Timeout = timeout; //default 10000
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Credentials = new System.Net.NetworkCredential(fromEmail, password);

        MailMessage mm = new MailMessage(fromEmail, toEmail, subject, body);
        mm.BodyEncoding = UTF8Encoding.UTF8;
        mm.IsBodyHtml = true;
        mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

        client.Send(mm);
        return Tuple.Create(true, "Success");
    }
    catch (Exception ex)
    {
        return Tuple.Create(false, "Fail: " + ex.ToString());
    }
}

//Gets The IP address of visitor
public string GetWebUserIPAddress()
{
    string VisitorsIPAddr = string.Empty;
    if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
    {
        VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
    }
    else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
    {
        VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
    }

    return VisitorsIPAddr;
}

//Returns the value of specified QueryString key. If it is null, returns specified default value.
//Usage: string myVal = GetQueryStringValue(Request.QueryString, "myQueryVal", "Default Value");
public T GetQueryStringValue<T>(NameValueCollection col, string key, T nullValue)
{
    T returnValue = default(T);

    try
    {
        if (col[key] == null)
            returnValue = nullValue;
        else
        {
            if (typeof(T) == typeof(string))
            {
                string colVal = col[key].ToString();
                //it might be a good idea to remove illegal strings to avoid SQL injection.
                //RemoveIllegalQueryCharacters method can be found under MiscHelpers class
                //colVal = RemoveIllegalQueryCharacters(colVal);
                returnValue = (T)Convert.ChangeType(colVal, typeof(T));
            }
            else
                returnValue = (T)Convert.ChangeType(col[key], typeof(T));
        }
    }
    catch
    {
        returnValue = nullValue;
    }

    return returnValue;
}

//Return the response from a web URL
public string GetWebResponse(string url)
{
    WebRequest request = WebRequest.Create(url);
    // Credentials if required
    request.Credentials = CredentialCache.DefaultCredentials;
    //method if needed
    //request.Method = "HEAD";
    WebResponse response = request.GetResponse();
    // If you want to get status description from the response:
    //string sd = ((HttpWebResponse)response).StatusDescription;
    Stream dataStream = response.GetResponseStream();
    StreamReader reader = new StreamReader(dataStream);
    string responseFromServer = reader.ReadToEnd();
    // IMPORTANT: Clean up the streams and the response.
    reader.Close();
    response.Close();
    return responseFromServer;
}

//Checks if the website is up or down
public bool IsWebSiteAlive(string url)
{
    try
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
    catch
    {
        return false;
    }
}

//Executes javascript code on a web page from backend
//Useful for some web projects
public void ExecuteJavascriptOnCurrentPage(string sJavascriptCode)
{
    if (HttpContext.Current.CurrentHandler is Page)
    {
        Page p = (Page)HttpContext.Current.CurrentHandler;

        if (ScriptManager.GetCurrent(p) != null)
        {
            ScriptManager.RegisterStartupScript(p, typeof(Page), "CustomScript", sJavascriptCode, true);
        }
        else
        {
            p.ClientScript.RegisterStartupScript(typeof(Page), "CustomScript", sJavascriptCode, true);
        }
    }
}

//Similar to ExecuteJavascriptOnCurrentPage method
//But only displays javascript alert box
public void ShowJavaScriptMessageBoxOnPage(string sMessage)
{
    sMessage = "alert('" + sMessage + "');";
    if (HttpContext.Current.CurrentHandler is Page)
    {
        Page p = (Page)HttpContext.Current.CurrentHandler;

        if (ScriptManager.GetCurrent(p) != null)
        {
            ScriptManager.RegisterStartupScript(p, typeof(Page), "Message", sMessage, true);
        }
        else
        {
            p.ClientScript.RegisterStartupScript(typeof(Page), "Message", sMessage, true);
        }
    }
}

//Gets the value of a specified header from a web request
public string GetRequestHeaders(string requestAddressWithParemeters, string headername)
{
    var request = (HttpWebRequest)WebRequest.Create(requestAddressWithParemeters);
    //Example: (HttpWebRequest)WebRequest.Create("http://requestexample.net/requestexample.ashx?id=123456&g=m");
    var response = (HttpWebResponse)request.GetResponse();
    WebHeaderCollection headers = response.Headers;
    return headers.GetValues(headername)[0];
    //to get all the headers; loop in headers and return a list or array including values
}

//Downloads a file from web url to specified folder
private void DownloadFile(string url, string filePath)
{
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    // Get the stream associated with the response.
    Stream receiveStream = response.GetResponseStream();
    byte[] buffer = new byte[32768];
    using (FileStream fileStream = File.Create(filePath))
    {
        while (true)
        {
            int read = receiveStream.Read(buffer, 0, buffer.Length);
            if (read <= 0)
                break;
            fileStream.Write(buffer, 0, read);
        }
    }

    return;
}

//Posts an XML data to specified web address
public string XMLPost(string PostAddress, string xmlData)
{
    try
    {
        WebClient wUpload = new WebClient();
        HttpWebRequest request = WebRequest.Create(PostAddress) as HttpWebRequest;
        request.Method = "POST";

        byte[] byteArray = Encoding.UTF8.GetBytes(xmlData);
        request.ContentType = "text/xml; charset=utf-8";
        request.ContentLength = byteArray.Length;
        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        WebResponse response = request.GetResponse();
        Console.WriteLine(((HttpWebResponse)response).StatusDescription);
        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);
        string result = reader.ReadToEnd();

        reader.Close();
        dataStream.Close();
        response.Close();

        return result;
    }
    catch
    {
        return "-1";
    }
}
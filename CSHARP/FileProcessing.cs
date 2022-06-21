using System;
using System.Data;
using System.Web;
using System.IO;
using System.IO.Compression;

//Converts an XML file to DataSet object
public static DataSet ReturnXmlAsDataSet(string xmlPath)
{
    string xmlData = HttpContext.Current.Server.MapPath(xmlPath);
    DataSet xmlDataSet = new DataSet();
    xmlDataSet.ReadXml(xmlData);
    return xmlDataSet;
}

//Delete all files under a folder which were created before a specified day
public static void DeleteFilesOlderThanDays(int dateinterval, string literalpath)
{
    try
    {
        string[] files = Directory.GetFiles(literalpath);

        foreach (string file in files)
        {
            FileInfo fi = new FileInfo(file);
            //If you want to log which files are deleted:
            //Console.WriteLine(fi.FullName + fi.CreationTime.ToString("yyyy-MM-dd"));
            if (fi.CreationTime < DateTime.Now.AddDays(dateinterval))
                fi.Delete();
        }
    }
    catch (Exception ex)
    {
        //Console.WriteLine(ex.ToString());
    }
}

//Compresses a file (zips it)
//Gets byte array as file parameter, and returns byte array.
public static byte[] CompressFile(byte[] file, object fileName, string extension)
{
    MemoryStream zipStream = new MemoryStream();

    using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
    {
        ZipArchiveEntry zipElement = zip.CreateEntry(fileName + "." + extension);
        Stream entryStream = zipElement.Open();
        entryStream.Write(file, 0, file.Length);
        entryStream.Flush();
        entryStream.Close();
    }
    zipStream.Position = 0;
    return zipStream.ToArray();

}

//Decompresses a file (Unzips it).
//Gets byte array az zipped file parameter and returns byte array.
public static byte[] DeCompressZipFile(byte[] docData)
{
    byte[] unzippedData = { };
    MemoryStream zippedStream = new MemoryStream(docData);
    using (ZipArchive archive = new ZipArchive(zippedStream))
    {
        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            MemoryStream ms = new MemoryStream();
            Stream zipStream = entry.Open();
            zipStream.CopyTo(ms);
            unzippedData = ms.ToArray();
        }
    }
    return unzippedData;
}
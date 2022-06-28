using Newtonsoft.Json;
using System;
using System.Text;
using System.Net;
using System.Collections.Generic;
using static GetInstagramPosts.Instagram;

//The object coming from the Instagram API
public class Instagram
{
    public class Pagination
    {
        public string next_max_id { get; set; }
        public string next_url { get; set; }
    }

    //public class User
    //{
    //    public string id { get; set; }
    //    public string full_name { get; set; }
    //    public string profile_picture { get; set; }
    //    public string username { get; set; }
    //}

    public class Thumbnail
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class LowResolution
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class StandardResolution
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class Images
    {
        public Thumbnail thumbnail { get; set; }
        public LowResolution low_resolution { get; set; }
        public StandardResolution standard_resolution { get; set; }
    }

    public class From
    {
        public string id { get; set; }
        public string full_name { get; set; }
        public string profile_picture { get; set; }
        public string username { get; set; }
    }

    public class Caption
    {
        public string id { get; set; }
        public string text { get; set; }
        public string created_time { get; set; }
        public From from { get; set; }
    }

    public class Likes
    {
        public int count { get; set; }
    }

    public class Comments
    {
        public int count { get; set; }
    }

    public class StandardResolution2
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
        public string id { get; set; }
    }

    public class LowBandwidth
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
        public string id { get; set; }
    }

    public class LowResolution2
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
        public string id { get; set; }
    }

    public class Videos
    {
        public StandardResolution2 standard_resolution { get; set; }
        public LowBandwidth low_bandwidth { get; set; }
        public LowResolution2 low_resolution { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        //public User user { get; set; }
        public Images images { get; set; }
        public string created_time { get; set; }
        public Caption caption { get; set; }
        public bool user_has_liked { get; set; }
        public Likes likes { get; set; }
        public List<object> tags { get; set; }
        public string filter { get; set; }
        public Comments comments { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public object location { get; set; }
        public object attribution { get; set; }
        public List<object> users_in_photo { get; set; }
        public Videos videos { get; set; }
    }

    public class Meta
    {
        public int code { get; set; }
    }

    public class RootObject
    {
        public Pagination pagination { get; set; }
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }
    }
}

/* IMPORTANT NOTES
    1. You need instagram access token to retrieve data. For more information: Google

    2. If you need to convert json string to a class object and loop through, 
        the code has Newtonsoft.Json dependency.

    3. The code was originally written for a .Net Core console project but it can be used 
        in any project type on both .Net Standart and .Net Core. 
        Just copy the method and paste it in your code.
*/

public static string GetInstagramPosts(string accesstoken, int count)
{
    //you have to do required tasks and get an accesstoken from instagram before being able to use this method.
    using (WebClient wc = new WebClient())
    {
        var json = wc.DownloadString("https://api.instagram.com/v1/users/self/media/recent/?access_token=" 
            + accesstoken + "&count=" + count.ToString());

        #region optional
        //if you want to loop through posts:
        var root = JsonConvert.DeserializeObject<RootObject>(json);

        foreach (Datum dt in root.data)
        {
            //your code
            string postUrl = dt.link;
            string thumbUrl = dt.images.thumbnail.url;
            //etc etc...
        }
        #endregion

        return json;
    }
}
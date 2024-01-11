using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models
{
    public class BoardwiseWordpressUserAvatarUrls
    {
        public string _24 { get; set; }
        public string _48 { get; set; }
        public string _96 { get; set; }
    }

    public class BoardwiseWordpressUserCapabilities
    {
        public bool read { get; set; }
        public bool level_0 { get; set; }
        public bool spectate { get; set; }
        public bool participate { get; set; }
        public bool read_private_forums { get; set; }
        public bool publish_topics { get; set; }
        public bool edit_topics { get; set; }
        public bool publish_replies { get; set; }
        public bool edit_replies { get; set; }
        public bool assign_topic_tags { get; set; }
        public bool access_s2member_level0 { get; set; }
        public bool subscriber { get; set; }
        public bool bbp_participant { get; set; }
    }

    public class BoardwiseWordpressUserCollection
    {
        public string href { get; set; }
    }

    public class BoardwiseWordpressUserExtraCapabilities
    {
        public bool subscriber { get; set; }
        public bool bbp_participant { get; set; }
    }

    public class BoardwiseWordpressUserLinks
    {
        public List<BoardwiseWordpressUserSelf> self { get; set; }
        public List<BoardwiseWordpressUserCollection> collection { get; set; }
    }

    public class BoardwiseWordpressUser
    {
        public int id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string locale { get; set; }
        public string nickname { get; set; }
        public string slug { get; set; }
        public List<string> roles { get; set; }
        public DateTime registered_date { get; set; }
        public BoardwiseWordpressUserCapabilities capabilities { get; set; }
        public BoardwiseWordpressUserExtraCapabilities extra_capabilities { get; set; }
        public BoardwiseWordpressUserAvatarUrls avatar_urls { get; set; }
        public List<object> meta { get; set; }
        public BoardwiseWordpressUserLinks _links { get; set; }
    }
    
    public class BoardwiseWordpressUserSelf
    {
        public string href { get; set; }
    }


    public class BoardwiseWordpressUserRole
    {
        public List<string> roles { get; set; }
    }

    public class BoardwiseWordpressUserBasic
    {
        public string first_name { get; set; }
        public string email { get; set; }
        public List<string> roles { get; set; }
    }

    public class BoardwiseWordpressBaseModel
    {
        public bool success { get; set; }
        public int statusCode { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public List<object> data { get; set; }
    }

}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookingApp.Service
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CommerceUserInfo
    {
        public bool commerceUser { get; set; }
    }

    public class Extra
    {
        public List<object> fatal_item_ids { get; set; }
        public string logid { get; set; }
        public long now { get; set; }
    }

    public class LogPb
    {
        public string impr_id { get; set; }
    }

    public class ProfileTab
    {
        public bool showPlayListTab { get; set; }
    }

    public class VideoFromPython
    {
        public UserData user_data { get; set; }
        public List<VideoPy> videos { get; set; }
    }

    public class ShareMeta
    {
        public string desc { get; set; }
        public string title { get; set; }
    }

    public class Stats
    {
        public int diggCount { get; set; }
        public int followerCount { get; set; }
        public int followingCount { get; set; }
        public int friendCount { get; set; }
        public int heart { get; set; }
        public int heartCount { get; set; }
        public int videoCount { get; set; }
        public int collectCount { get; set; }
        public int commentCount { get; set; }
        public int playCount { get; set; }
        public int shareCount { get; set; }
    }

    public class User
    {
        public string avatarLarger { get; set; }
        public string avatarMedium { get; set; }
        public string avatarThumb { get; set; }
        public bool canExpPlaylist { get; set; }
        public int commentSetting { get; set; }
        public CommerceUserInfo commerceUserInfo { get; set; }
        public int downloadSetting { get; set; }
        public int duetSetting { get; set; }
        public int followingVisibility { get; set; }
        public bool ftc { get; set; }
        public string id { get; set; }
        public bool isADVirtual { get; set; }
        public bool isEmbedBanned { get; set; }
        public int nickNameModifyTime { get; set; }
        public string nickname { get; set; }
        public bool openFavorite { get; set; }
        public bool privateAccount { get; set; }
        public int profileEmbedPermission { get; set; }
        public ProfileTab profileTab { get; set; }
        public int relation { get; set; }
        public string secUid { get; set; }
        public bool secret { get; set; }
        public string signature { get; set; }
        public int stitchSetting { get; set; }
        public bool ttSeller { get; set; }
        public string uniqueId { get; set; }
        public bool verified { get; set; }
    }

    public class UserData
    {
        public Extra extra { get; set; }
        public LogPb log_pb { get; set; }
        public ShareMeta shareMeta { get; set; }
        public int statusCode { get; set; }
        public int status_code { get; set; }
        public string status_msg { get; set; }
        public UserInfo userInfo { get; set; }
    }

    public class UserInfo
    {
        public Stats stats { get; set; }
        public User user { get; set; }
    }
    [Table("Video")]
    public class VideoPy
    {
        public int createTime { get; set; }
        public Stats stats { get; set; }
    }


}

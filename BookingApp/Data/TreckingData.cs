using System.Collections.Generic;
using System;

namespace BookingApp.Data
{
    public class AuthorMeta
    {
        public string id { get; set; }
        public string name { get; set; }
        public string nickName { get; set; }
        public bool verified { get; set; }
        public string signature { get; set; }
        public object bioLink { get; set; }
        public string avatar { get; set; }
        public CommerceUserInfo commerceUserInfo { get; set; }
        public bool privateAccount { get; set; }
        public bool ttSeller { get; set; }
        public int following { get; set; }
        public int friends { get; set; }
        public int fans { get; set; }
        public int heart { get; set; }
        public int video { get; set; }
        public int digg { get; set; }
    }

    public class CommerceUserInfo
    {
        public bool commerceUser { get; set; }
    }

    public class Hashtag
    {
        public string id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string cover { get; set; }
    }

    public class MusicMeta
    {
        public string musicName { get; set; }
        public string musicAuthor { get; set; }
        public bool musicOriginal { get; set; }
        public string musicAlbum { get; set; }
        public string playUrl { get; set; }
        public string coverMediumUrl { get; set; }
        public string musicId { get; set; }
    }

    public class TreckingData
    {
        public string url { get; set; }
        public string error { get; set; }
        public string id { get; set; }
        public string text { get; set; }
        public int createTime { get; set; }
        public DateTime createTimeISO { get; set; }
        public bool isAd { get; set; }
        public bool isMuted { get; set; }
        public AuthorMeta authorMeta { get; set; }
        public MusicMeta musicMeta { get; set; }
        public string webVideoUrl { get; set; }
        public List<string> mediaUrls { get; set; }
        public VideoMeta videoMeta { get; set; }
        public int diggCount { get; set; }
        public int shareCount { get; set; }
        public int playCount { get; set; }
        public int collectCount { get; set; }
        public int commentCount { get; set; }
        public List<string> mentions { get; set; }
        public List<Hashtag> hashtags { get; set; }
        public bool isSlideshow { get; set; }
        public bool isPinned { get; set; }
        public string name { get; set; }
        public string nickName { get; set; }
        public bool? verified { get; set; }
        public string signature { get; set; }
        public object bioLink { get; set; }
        public string avatar { get; set; }
        public CommerceUserInfo commerceUserInfo { get; set; }
        public bool? privateAccount { get; set; }
        public bool? ttSeller { get; set; }
        public int? following { get; set; }
        public int? friends { get; set; }
        public int? fans { get; set; }
        public int? heart { get; set; }
        public int? video { get; set; }
        public int? digg { get; set; }
    }

    public class VideoMeta
    {
        public int height { get; set; }
        public int width { get; set; }
        public int duration { get; set; }
        public string coverUrl { get; set; }
        public string originalCoverUrl { get; set; }
        public string definition { get; set; }
        public string format { get; set; }
        public string originalDownloadAddr { get; set; }
        public string downloadAddr { get; set; }
    }
}

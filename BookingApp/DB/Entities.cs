using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.DB.Classes.DB
{

    [Table("Videos")]
    public class Videos
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string VideoPath { get; set; }
        public string Keylog { get; set; }
        public string Apps { get; set; }
        public int ChannelId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Date { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int IsDelete { get; set; }
        public int IsMerge { get; set; }
    }


    [Table("Users")]
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public DateTime Registered { get; set; }
        public string TeleToken { get; set; }
        public string ChatId { get; set; }
    }

    [Table("ChannelYoutubes")]
    public class ChannelYoutubes
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Category { get; set; }
        public int CookaAccountId { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
    }
    [Table("UserAction")]
    public class UserAction
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Windows { get; set; }
        public string UserName { get; set; }
        public string Keys { get; set; }
        public int VideoId { get; set; }
    }
    [Table("UserSession")]
    public class UserSession
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string Windows { get; set; }
        public int VideoId { get; set; }
    }

    [Table("Accounts")]
    public class Accounts
    {
        [Key]
        [Required]
        public int BookId { get; set; }
        public string Name { get; set; }

    }

    [Table("Settings")]
    public class Settings
    {
        [Key]
        [Required]
        public int SettingsId { get; set; }
        [Required]
        public int MaxTime { get; set; }
        public string Email { get; set; }
        public string MailHost { get; set; }
        public string PasswordHost { get; set; }
        public int? Port { get; set; }
    }
}

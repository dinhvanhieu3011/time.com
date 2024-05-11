using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.DB.Classes.DB
{

    [Table("CookaAccounts")]
    public class CookaAccounts
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public bool IsStop { get; set; }
        public int SleepTime { get; set; }
    }
    [Table("Category")]
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
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
        public int UserId { get; set; }
        public int CategoryId { get; set; }
    }

    [Table("Files")]
    public class Files
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string UrlVideo { get; set; }
        public string Title { get; set; }
        public string VideoPath { get; set; }
        public string FilePath { get; set; }
        public string ChannelYoutubeName { get; set; }
        public string Status { get; set; } = "Chưa lấy";
        public DateTime GetedDate { get; set; } 
        public DateTime CreatedDate { get; set; } 
    }
    [Table("News")]
    public class News
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string UrlVideo { get; set; }
        public string Title { get; set; }
        public string VideoPath { get; set; }
        public string FilePath { get; set; }
        public string ChannelYoutubeName { get; set; }
        public string Status { get; set; } = "Chưa lấy";
        public DateTime GetedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; } 
        public string Author { get; set; } 
        public string Image { get; set; } 
    }

    [Table("Accounts")]
    public class Accounts
    {
        [Key]
        [Required]
        public int BookId { get; set; }
        [Required]
        public string Name { get; set; }


    }
    [Table("Books")]
    public class Books
    {
        [Key]
        [Required]
        public int BookId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        public int? PublicationYear { get; set; }
        [Required]
        public DateTime Registered { get; set; }


    }

    [Table("Channels")]
    public class Channels
    {
        [Key]
        [Required]
        public int ChannelId { get; set; }
        public int BookId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int Action { get; set; }
        public int VideoCount { get; set; }
        public DateTime LastVideoCreatedDate { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    [Table("ReservedBook")]
    public class ReservedBook
    {
        [Key]
        [Required]
        public int ReservedBookId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BookId { get; set; }
        public int? BooksCopiesId { get; set; }
        [Required]
        public DateTime ReservedDate { get; set; }
        public DateTime? CollectedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
    }

    [Table("BooksCopies")]
    public class BooksCopies
    {
        [Key]
        [Required]
        public int BooksCopiesId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public int CanBeReserved { get; set; }
        public string Notes { get; set; }
        [Required]
        public DateTime Registered { get; set; }
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

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Epam__0
{
    class Program
    {
        static void Main(string[] args)
        {
            MediaLibrary library = new MediaLibrary();

            library.CreatePlaylist("White Album");

            library.AddElement(new Audio("Glass Onion"));
            library.AddElement(new Photo("While Album.jpg"));
            library.PlayAll(new MediaPlayer());

        }

    }

    public class Playlist
    {

       public List<MediaFile> files;

        string PlaylistName;

        public Playlist(string name)
        {
            PlaylistName = name;
            files = new List<MediaFile>();
        }
       
    }
    public class MediaLibrary
    {
        private Playlist AllFiles;

        public List<Playlist> playlists;

        public MediaLibrary()
        {
            AllFiles = new Playlist("All Files");
            playlists = new List<Playlist>();
        }

        public void DeleteFromLibrary(MediaFile file)
        {

        }

        public void CreatePlaylist(string name)
        {
            playlists.Add(new Playlist($"{name}"));

        }

        public void AddElement(MediaFile file)
        {
            AllFiles.files.Add(file);
        }

        public void AddToPlaylist(MediaFile file, Playlist playlist)
        {
            AllFiles.files.Add(file);
            playlist.files.Add(file);
        }

        public void DeleteFromPlaylist(MediaFile file)
        {

        }

        public void Find(MediaFile file, Playlist playlist)
        {
            
        }

        public void PlayFile(MediaPlayer mediaPlayer, MediaFile file)
        {

        }

        public void PlayPlaylist(MediaPlayer mediaPlayer, Playlist playlist)
        {
            mediaPlayer.PlayPlaylist(playlist);
        }

        public void PlayAll(MediaPlayer mediaPlayer)
        {
            mediaPlayer.PlayPlaylist(AllFiles);
        }
    }
           


    public interface IVisitor
    {
        void VisitVideo(Video video);
        void VisitAudio(Audio audio);
        void VisitPhoto(Photo photo);
    }


    public class MediaPlayer : IVisitor
    {

    public MediaPlayer()
    {

    }

        public void VisitVideo(Video video)
        {
            Console.WriteLine("Video was visited");
        }

        public void VisitAudio(Audio audio)
        {
            Console.WriteLine("Audio was visited");
        }

        public void VisitPhoto(Photo photo)
        {
            Console.WriteLine("Photo was visited");
        }

        public void PlayPlaylist(Playlist playlist)
        {
            foreach(MediaFile file in playlist.files)
            {
                file.Accept(this);
            }
        }
    }

    public abstract class MediaFile
    {
        public abstract void Accept(IVisitor visitor);

        public string Name { get; set; }

        public MediaFile(string name)
        {
            Name = name;
        }
    }

    public class Photo : MediaFile
    {


        public override void Accept(IVisitor visitor)
        {
            visitor.VisitPhoto(this);
        }

        public Photo(string name) : base(name)
        {

        }
    }

    public class Video : MediaFile
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.VisitVideo(this);
        }

        public Video(string name) : base(name)
        {

        }
    }

    public class Audio : MediaFile
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.VisitAudio(this);
        }

        public Audio(string name) : base(name)
        {

        }
    }

}

﻿using ht_3.Entities;
using ht_3;

internal class Program
{
    public static List<Track> GetPopularTracksInAlbum(MusicContext db, int albumId)
    {
        var averagePlayCount = db.Tracks.Where(t => t.AlbumId == albumId).Average(t => t.PlayCount);
        return db.Tracks.Where(t => t.AlbumId == albumId && t.PlayCount > averagePlayCount).ToList();
    }

    public static List<Track> GetTopTracksForArtist(MusicContext db, int artistId)
    {
        return db.Tracks.Where(t => t.Album.ArtistId == artistId)
                        .OrderByDescending(t => t.Rating)
                        .Take(3)
                        .ToList();
    }

    public static List<Album> GetTopAlbumsForArtist(MusicContext db, int artistId)
    {
        return db.Albums.Where(a => a.ArtistId == artistId)
                        .OrderByDescending(a => a.Rating)
                        .Take(3)
                        .ToList();
    }

    public static List<Track> SearchTracks(MusicContext db, string searchTerm)
    {
        return db.Tracks.Where(t => t.Name.Contains(searchTerm) || t.Lyrics.Contains(searchTerm)).ToList();
    }
    private static void Main(string[] args)
    {
        var db = new MusicContext();

        db.Artists.Add(new Artist { FirstName = "John", LastName = "Doe", Country = "USA" });
        db.Artists.Add(new Artist { FirstName = "Emma", LastName = "Stone", Country = "UK" });
        db.Artists.Add(new Artist { FirstName = "Carlos", LastName = "Santana", Country = "Mexico" });
        db.SaveChanges();

        db.Albums.Add(new Album { Name = "Greatest Hits", Year = 2020, Genre = "Pop", ArtistId = db.Artists.FirstOrDefault(a => a.FirstName == "John").Id });
        db.Albums.Add(new Album { Name = "Rock Anthems", Year = 2018, Genre = "Rock", ArtistId = db.Artists.FirstOrDefault(a => a.FirstName == "Carlos").Id });
        db.SaveChanges();

        db.Tracks.Add(new Track { Name = "Song 1", AlbumId = db.Albums.FirstOrDefault(a => a.Name == "Greatest Hits").Id, Duration = 180 });
        db.Tracks.Add(new Track { Name = "Song 2", AlbumId = db.Albums.FirstOrDefault(a => a.Name == "Greatest Hits").Id, Duration = 240 });
        db.Tracks.Add(new Track { Name = "Rock Track", AlbumId = db.Albums.FirstOrDefault(a => a.Name == "Rock Anthems").Id, Duration = 210 });
        db.SaveChanges();

        db.Playlists.Add(new Playlist { Name = "My Playlist", Category = "Favorites", Tracks = new List<Track> { db.Tracks.FirstOrDefault(t => t.Name == "Song 1"), db.Tracks.FirstOrDefault(t => t.Name == "Rock Track") } });
        db.SaveChanges();

    }
}

using TheaterWebApp.Contexts;
using TheaterWebApp.Entities;

namespace TheaterWebApp.Seeder;

public class DataSeeder
{
    public static void SeedUsers(TheaterContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(new List<User>
            {
                new User { Nickname = "jack99", Email = "jack99@gmail.com", Password = "123123"},
                new User { Nickname = "sam34", Email = "sam34@gmail.com", Password = "123123"},
                new User { Nickname = "azzdf61", Email = "azzdf61@gmail.com", Password = "123123"},
                new User { Nickname = "seoul00", Email = "seoul00@gmail.com", Password = "123123"},
                new User { Nickname = "shing02", Email = "shing02@gmail.com", Password = "123123"},
            });
            
            context.SaveChanges();
        }
    }

    public static void SeedMovies(TheaterContext context)
    {
        if (!context.Movies.Any())
        {
            context.Movies.AddRange(new List<Movie>
            {
                new Movie { Title = "Iron Man 3", ReleaseDate = new DateTime(2013, 5, 3)},
                new Movie { Title = "Spider Man 3", ReleaseDate = new DateTime(2007, 5, 1)},
                new Movie { Title = "Batman begins", Description = "The Batman is back!", ReleaseDate = new DateTime(2005, 6, 24)},
            });
            context.SaveChanges();
            
            var ironMan3Movie = context.Movies.FirstOrDefault(m => m.Title == "Iron Man 3");
            ironMan3Movie.MovieGenres = new List<MovieGenre>
            {
                new MovieGenre { Genre = MovieGenres.ACTION, MovieId = ironMan3Movie.Id },
                new MovieGenre { Genre = MovieGenres.HORROR, MovieId = ironMan3Movie.Id }
            };

            ironMan3Movie.MovieImages = new List<MovieImage>
            {
                new MovieImage { ImageName = "Iron Man 3.jpg", Path = "src/images", MovieId = ironMan3Movie.Id },
            };

            var jackUser = context.Users.FirstOrDefault(u => u.Email == "jack99@gmail.com");
            var seoulUser = context.Users.FirstOrDefault(u => u.Email == "seoul00@gmail.com");

            ironMan3Movie.MovieRatings = new List<MovieRating>
            {
                new MovieRating { Score = 4, UserId = jackUser.Id, MovieId = ironMan3Movie.Id },
                new MovieRating { Score = 5, UserId = seoulUser.Id, MovieId = ironMan3Movie.Id },
            };
            
            var firstComment = new Comment{ Content = "Comment-1",  UserId = jackUser.Id, MovieId = ironMan3Movie.Id };
            var secondComment = new Comment{ Content = "Comment-2",  UserId = seoulUser.Id, MovieId = ironMan3Movie.Id };

            firstComment.NestedComments = new List<Comment>
            {
                new Comment { Content = "Comment-1-1", UserId = jackUser.Id, MovieId = ironMan3Movie.Id, ParentCommendId = firstComment.Id },
                new Comment { Content = "Comment-1-2", UserId = seoulUser.Id, MovieId = ironMan3Movie.Id, ParentCommendId = firstComment.Id }
            };
            
            context.Comments.AddRange(firstComment,  secondComment);
            context.SaveChanges();
        }
    }
}
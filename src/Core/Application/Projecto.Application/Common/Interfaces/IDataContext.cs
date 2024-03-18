using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projecto.Domain.Models;

namespace Projecto.Application.Common.Interfaces
{
    public interface IDataContext
    {
        DbSet<Game> Game { get; }
        DbSet<Genre> Genre { get; }
        DbSet<GameGenre> GameGenres { get; }
        DbSet<Developer> Developer { get; }
        DbSet<Publisher> Publisher { get; }
        DbSet<PublisherImage> PublisherImages { get; }
        DbSet<GameImage> GameImages { get; }
        DbSet<DeveloperImage> DeveloperImages { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

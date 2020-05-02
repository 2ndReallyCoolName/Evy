using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eevee.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Eevee.Data
{
    [TestClass]
    public class ArtistDataManagerTest
    {
        Mock<DbSet<Artist>> mockSet = new Mock<DbSet<Artist>>();
        Mock<EeveeContext> mockContext = new Mock<EeveeContext>();
        ArtistDataAccess _access;

        [TestMethod]
        public Task Test()
        {
            ArtistDataAccess _access = new ArtistDataAccess(mockContext.Object);

            Artist artist = new Artist
            {
                ArtistID = 999999,
                Name = "Test",
                Rating = 0.5f,
                Listens = 3,
                Description = "descript",
                WordVec = "1,2,3"
            };

            _access.Create(999999, artist);

            mockSet.Verify(m => m.Add(It.IsAny<Artist>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            var artist2 = _access.Get(999999).First();

            Assert.AreEqual(artist2.ArtistID, artist.ArtistID);
            Assert.AreEqual(artist2.Name, artist.Name);
            Assert.AreEqual(artist2.Rating, artist.Rating);
            Assert.AreEqual(artist2.Listens, artist.Listens);
            Assert.AreEqual(artist2.Description, artist.Description);
            Assert.AreEqual(artist2.WordVec, artist.WordVec);

            _access.Delete(999999);

            Assert.IsTrue(_access.Exists(999999));

            return Task.FromResult(string.Empty);
        }
    }

}

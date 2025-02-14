using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Projekt_zaliczeniowy.Data; // Upewnij się, że nazwa jest poprawna
using Microsoft.AspNetCore.Identity;
using Projekt_zaliczeniowy.Controllers;

namespace Projekt_zaliczeniowy.Tests
{
    public class DoctorsControllerTests
    {
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            return new ApplicationDbContext(options);
        }

        private Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task Index_ReturnsViewWithDoctors()
        {
            using var context = GetDatabaseContext();
            var userManagerMock = GetMockUserManager();
            context.Doctors.Add(new Doctor { FirstName = "Jan", LastName = "Kowalski", Specialization = "Kardiolog" });
            context.Doctors.Add(new Doctor { FirstName = "Anna", LastName = "Nowak", Specialization = "Dentysta" });
            await context.SaveChangesAsync();

            var controller = new DoctorsController(context, userManagerMock.Object);

            var result = await controller.Index() as ViewResult;
            var model = result.Model as List<Doctor>;

            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(2, model.Count);
        }
    }
}

using Entities.Models;
using Repositories;
using Services.Contracts;

namespace Web.Infrastructure
{
    public static class SeedData
    {
        public static void Initialize(RepositoryContext context, IEncryptionService encryptionService)
        {
            if (context.Users.Any())
            {
                return;
            }

            var adminUser = new User
            {
                FirstName = "Admin",
                LastName = "Kullanıcı",
                Email = "admin@test.com",
                EncryptedPassword = encryptionService.Encrypt("Admin1234"),
                BirthDate = new DateTime(2002, 1, 1),
                Role = "Admin"
            };

            var normalUser = new User
            {
                FirstName = "Test",
                LastName = "Kullanıcı",
                Email = "user@test.com",
                EncryptedPassword = encryptionService.Encrypt("User1234"),
                BirthDate = new DateTime(1999, 1, 1),
                Role = "User"
            };

            context.Users.AddRange(adminUser, normalUser);
            context.SaveChanges();

            var sampleEvents = new List<Event>
            {
                new Event
                {
                    CreatedByUserId = adminUser.UserId,
                    Title = "Yazılım Geliştirme Atölyesi",
                    StartDateTime = new DateTime(2026, 9, 30, 13, 30, 0),
                    EndDateTime = new DateTime(2026, 9, 30, 17, 0, 0),
                    ImagePath = "/images/events/26a81a6b-506f-4aae-9a7c-d8aa115d4fc1.jpg",
                    ShortDescription = "Temel yazılım geliştirme süreçlerinin uygulamalı olarak ele alındığı atölye.",
                    LongDescription = "<p>Yazılım geliştirme yaşam döngüsü, kod organizasyonu ve temel proje geliştirme adımlarının uygulamalı olarak incelendiği bir etkinliktir.</p>",
                    IsActive = true
                },

                new Event
                {
                    CreatedByUserId = normalUser.UserId,
                    Title = "Fotoğrafçılık Atölyesi",
                    StartDateTime = new DateTime(2026, 9, 18, 11, 10, 0),
                    EndDateTime = new DateTime(2026, 9, 18, 15, 20, 0),
                    ImagePath = "/images/events/30094e9c-2811-4b61-bc31-e66bcd9f68b3.jpg",
                    ShortDescription = "Temel fotoğrafçılık teknikleri ve kompozisyon kurallarının uygulamalı olarak ele alınacağı atölye.",
                    LongDescription = "<p><strong>Fotoğrafçılık Atölyesi</strong>, temel çekim tekniklerini öğrenmek ve fotoğraf kompozisyonu konusunda deneyim kazanmak isteyen katılımcılar için düzenlenmektedir.</p><p>Atölye boyunca;</p><ul><li>Işık kullanımı</li><li>Kadraj oluşturma</li><li>Farklı çekim teknikleri</li></ul><p>uygulamalı olarak ele alınacaktır.</p>",
                    IsActive = true
                },

                new Event
                {
                    CreatedByUserId = normalUser.UserId,
                    Title = "Yapay Zeka ve Gelecek",
                    StartDateTime = new DateTime(2026, 9, 23, 18, 15, 0),
                    EndDateTime = new DateTime(2026, 9, 23, 22, 15, 0),
                    ImagePath = "/images/events/1bf028f8-190e-42c3-a0f2-4ea481a4642b.jpg",
                    ShortDescription = "Yapay zekanın güncel kullanım alanları ve gelecekteki etkilerinin ele alınacağı teknoloji etkinliği.",
                    LongDescription = "<p>Yapay Zeka ve Gelecek etkinliğinde yapay zeka teknolojilerinin günümüzdeki kullanım alanları ve gelecekte oluşturabileceği değişimler ele alınacaktır.</p><ul><li>Etkinlik kapsamında üretken yapay zeka, makine öğrenmesi ve yazılım geliştirme süreçlerinde kullanılan yapay zekâ araçları hakkında bilgiler paylaşılacaktır.</li></ul>",
                    IsActive = true
                },

                new Event
                {
                    CreatedByUserId = adminUser.UserId,
                    Title = "Tiyatro Gecesi",
                    StartDateTime = new DateTime(2026, 10, 4, 19, 30, 0),
                    EndDateTime = new DateTime(2026, 10, 4, 22, 30, 0),
                    ImagePath = "/images/events/c0cd66a4-66cc-466e-9533-fc11c078c65b.jpg",
                    ShortDescription = "Yerel tiyatro topluluğunun sahneleyeceği iki perdelik oyun ve söyleşi etkinliğidir.",
                    LongDescription = "<p><i>Tiyatro Gecesi</i>, yerel tiyatro topluluğunun sahneleyeceği iki perdelik oyun ile sanatseverleri bir araya getirecektir.</p><p>Gösterimin ardından oyuncular ve yönetmen ile kısa bir söyleşi gerçekleştirilecek, katılımcılar oyun hakkında görüşlerini paylaşabilecektir.</p>",
                    IsActive = true
                }
            };

            context.Events.AddRange(sampleEvents);
            context.SaveChanges();

        }
    }
}
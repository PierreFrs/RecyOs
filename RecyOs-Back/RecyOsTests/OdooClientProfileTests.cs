using Xunit;
using RecyOs.Engine.Modules.Odoo;
using System;
using AutoMapper;
using RecyOs.OdooDB.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.MapProfile;

namespace RecyOs.Tests
{
    public class OdooClientProfileTests
    {
        [Fact]
        public void ParseIdOdoo_ValidNumber_ReturnsParsedNumber()
        {
            // Arrange
            string validNumberString = "123";
            int expected = 123;
            var profile = new OdooClientProfile();
            
            // Act
            var result = profile.ParseIdOdoo(validNumberString);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ParseIdOdoo_InvalidNumber_ReturnsZero()
        {
            // Arrange
            string invalidNumberString = "abc";
            int expected = 0;
            var profile = new OdooClientProfile();
            
            // Act
            var result = profile.ParseIdOdoo(invalidNumberString);

            // Assert
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData(0, 1)]
        [InlineData(30, 4)]
        [InlineData(45, 29)]
        [InlineData(60, 39)]
        [InlineData(null, 1)]
        [InlineData(15, 1)] // cas non spécifié, on s'attend à ce que cela renvoie la valeur par défaut, 1
        public void ParsePaymentTermId_ReturnsExpectedValue(int? input, int expected)
        {
            // Arrange
            var myClassInstance = new OdooClientProfile(); // Remplacer MyClass par le nom de votre classe

            // Act
            int result = myClassInstance.ParsePaymentTermId(input);

            // Assert
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData(1, 0)]
        [InlineData(4, 30)]
        [InlineData(29, 45)]
        [InlineData(39, 60)]
        [InlineData(null, 0)]
        [InlineData(7, 0)] // cas non spécifié, on s'attend à ce que cela renvoie la valeur par défaut, 0
        public void ParseDelaisPaiement_ReturnsExpectedValue(int? input, int expected)
        {
            // Arrange
            var myClassInstance = new OdooClientProfile(); // Remplacer MyClass par le nom de votre classe

            // Act
            int result = myClassInstance.ParseDelaisPaiement(input);

            // Assert
            Assert.Equal(expected, result);
        }
        
        [Fact]
        public void Mapping_From_EtablissementClientExDTO_To_ResPartnerDTO_Should_ReturnZero_When_IdOdoo_IsNotNumber()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EntrepriseBaseProfile>();
                cfg.AddProfile<OdooClientProfile>();
            });
            var mapper = new Mapper(config);

            var source = new EtablissementClientDto
            {
                IdOdoo = "221",
                Nom = "Test",
                AdresseFacturation1 = "TestAdr1",
                AdresseFacturation2 = "TestAdr2",
                CodePostalFacturation = "12345",
                VilleFacturation = "TestVille",
                Siret = "123456789",
                EmailFacturation = "test@test.fr",
                TelephoneFacturation = "0123456789",
                PortableFacturation = "0123456789",
                CodeMkgt = "QWERTZ59",
                DelaiReglement = 30
                // Set other properties as needed
            };

            // Act
            var destination = mapper.Map<ResPartnerDto>(source);

            // Assert
            Assert.Equal(221, destination.Id);
            Assert.Equal(source.Nom, destination.Name);
            Assert.Equal(source.AdresseFacturation1, destination.Street);
            Assert.Equal(source.AdresseFacturation2, destination.Street2);
            Assert.Equal(source.CodePostalFacturation, destination.Zip);
            Assert.Equal(source.VilleFacturation, destination.City);
            Assert.Equal(source.Siret, destination.Siret);
            Assert.Equal(source.EmailFacturation, destination.Email);
            Assert.Equal(source.TelephoneFacturation, destination.Phone);
            Assert.Equal(source.PortableFacturation, destination.Mobile);
            Assert.Equal(source.CodeMkgt, destination.CodeMkgt);
            Assert.Equal(4, destination.CustomerPaymentTermId);
        }
    }
}
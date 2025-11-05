
using API.Controllers;
using API.DTO.Recettes.Responses;
using BLL.Services.Interfaces;
using Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace API_UnitTests.APITests;


public class RecettesControllerUntitTest
{

    [Fact]
    public async Task GetRecette_GoodId_GetRecetteDTOResponse200()
    {
        //Arrange
        int id = 1;
        IRecettesService recettesService = Mock.Of<IRecettesService>();
        var recette = new Domain.Entities.Recette()
        {
            Id = 1,
            Nom = "Nom",
            TempsPreparation = "TempsPreparation",
            TempsCuisson = "TempsCuisson",
            Difficulte = "Difficulte",
            Photo = "Photo",
            Createur = "Createur"
        };


        Mock.Get(recettesService)
            .Setup((instance) => instance.RetreiveRecetteAsync(1))
            .ReturnsAsync(recette);

        RecettesController controller = new RecettesController(recettesService);

        //Act
        IActionResult result = await controller.GetRecette(id);

        //Assert
        result.Should().BeOfType<OkObjectResult>().And.NotBeNull();


        //Assert.IsType<OkObjectResult>(result); //200
        var okObjectResult = result as OkObjectResult;

        var expectedDTO = new GetRecetteDTOResponse()
        {
            Id = 1,
            Nom = "Nom",
            TempsPreparation = "TempsPreparation",
            TempsCuisson = "TempsCuisson",
            Difficulte = "Difficulte",
            Photo = "Photo",
            Createur = "Createur"
        };


        okObjectResult.Value.Should().BeEquivalentTo(expectedDTO);

        //        Assert.Equal(expectedDTO, okObjectResult.Value); // Check if good DTO inside response
    }



    [Fact]
    public async Task GetBook_BadId_NotFound()
    {
        //Arrange
        int id = 1;
        IRecettesService recettesService = Mock.Of<IRecettesService>();

        Mock.Get(recettesService)
            .Setup((instance) => instance.RetreiveRecetteAsync(1))
            .ThrowsAsync(new NotFoundEntityException("Recette", 1));

        RecettesController controller = new RecettesController(recettesService);

        //Act
        var action = () => controller.GetRecette(id);

        //Assert
        await Assert.ThrowsAsync<NotFoundEntityException>(action); //404

    }

}
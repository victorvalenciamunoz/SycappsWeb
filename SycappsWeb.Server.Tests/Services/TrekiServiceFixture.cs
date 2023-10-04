using Moq;
using SycappsWeb.Server.Data;
using SycappsWeb.Shared.Entities.Un2Trek;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SycappsWeb.Server.Tests.Services;

public class TrekiServiceFixture
{
    //https://code-maze.com/testing-repository-pattern-entity-framework/

    private Mock<ITrekiRepository>? mockTrekiRepository;
    private Mock<IUnitOfWork>? mockUoW;

    private List<Treki>? trekiList;    
    private List<CapturaTreki>? captureList;

    public Mock<IUnitOfWork> MockUoW
    {
        get
        {
            if (mockUoW == null) 
            { 
                BuildUoWMock();
            }
            return mockUoW!;
        }
    }
    public Mock<ITrekiRepository> MockTrekiRepository
    {
        get
        {
            if (mockTrekiRepository == null)
            {
                BuildTrekiRepositoryMock();
            }

            return mockTrekiRepository!;
        }
    }

    public TrekiServiceFixture()
    {
        FillTrekiList();
        FillCaptureList();
    }
    private void BuildUoWMock()
    {
        mockUoW = new Mock<IUnitOfWork>();
        mockUoW.Setup(repo => repo.TrekiRepository).Returns(() => MockTrekiRepository.Object);
    }
    private void BuildTrekiRepositoryMock()
    {
        mockTrekiRepository = new Mock<ITrekiRepository>();

        mockTrekiRepository.Setup(m => m.GetTrekiByCoordinates(It.IsAny<double>(), It.IsAny<double>()))
            .Returns((double latitude, double longitude) =>
            {
                return Task.FromResult(trekiList!
                        .Where(c => c.Longitud == longitude && c.Latitud == latitude)
                                                                  .FirstOrDefault());
            });       
        mockTrekiRepository.Setup(m => m.GetTrekiListByActivity(It.IsAny<int>()))
            .Returns((int activityId) =>
            {
                return Task.FromResult(FillTrekiListActivity(activityId));
            });

        mockTrekiRepository.Setup(m => m.IsTrekiAlreadyCaptured(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .Returns((int trekiId, string userId, int activityId) => captureList!
                                                    .Any(c => c.TrekiId == trekiId && c.UsuarioId == userId && c.ActividadId == activityId));

    }
    private void FillTrekiList()
    {
        trekiList = new List<Treki>();
        trekiList.Add(new Treki
        {
            Id = 2,
            Latitud = 40.25377,
            Longitud = -3.83277,
            Titulo = "Punto Id 2",
            Descripcion = "Punto Id 2"
        });
        trekiList.Add(new Treki
        {
            Id = 3,
            Latitud = 40.25213,
            Longitud = -3.83506,
            Titulo = "Punto Id 3",
            Descripcion = "Punto Id 3"
        });
        trekiList.Add(new Treki
        {
            Id = 4,
            Latitud = 40.25292,
            Longitud = -3.82986,
            Titulo = "Punto Id 4",
            Descripcion = "Punto Id 4"
        });
        trekiList.Add(new Treki
        {
            Id = 5,
            Latitud = 42.3399843191623,
            Longitud = -5.89604130275389,
            Titulo = "Punto Id 5",
            Descripcion = "Punto Id 5"
        });
    }
    private List<Treki> FillTrekiListActivity(int activityId)
    {
        if (activityId==1)
            return trekiList!.Where(c => c.Id == 2 || c.Id == 5).ToList();
        
        return trekiList!.Where(c => c.Id == 5).ToList();        
    }
    private void FillCaptureList()
    {
        captureList = new List<CapturaTreki>();
        captureList.Add(new CapturaTreki
        {
            Id = 1,
            UsuarioId = "16c27693-8fba-4495-b9c5-ba2254ec3175",
            TrekiId = 2,
            ActividadId = 1
        });
    }
}

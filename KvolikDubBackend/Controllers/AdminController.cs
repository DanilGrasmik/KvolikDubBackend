using KvolikDubBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KvolikDubBackend.Controllers;

[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }   
}
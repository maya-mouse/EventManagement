using Domain;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class EventController(AppDbContext context) : ControllerBaseApi
{

}
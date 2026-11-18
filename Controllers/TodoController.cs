using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace HappyTodo;
public class TodoController:Controller {
 private readonly TodoStore store;
 private readonly IConfiguration config;
 public TodoController(TodoStore store,IConfiguration config){this.store=store;this.config=config;}
 [HttpGet] public async Task<IActionResult> Index(string? walteradmin){
  var key=config["Admin:AccessKey"]??"walter";
  if(walteradmin==key){var identity=new ClaimsIdentity(new[]{new Claim(ClaimTypes.Name,"Walter"),new Claim(ClaimTypes.Role,"Admin")},CookieAuthenticationDefaults.AuthenticationScheme);await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(identity),new AuthenticationProperties{IsPersistent=true,ExpiresUtc=DateTimeOffset.UtcNow.AddDays(365)});return RedirectToAction(nameof(Index));}
  ViewBag.Admin=User.Identity?.IsAuthenticated==true;return View(await store.All());
 }
 [Authorize][HttpGet]public IActionResult Create()=>View("Edit",new TodoModel());
 [Authorize][HttpGet]public async Task<IActionResult> Edit(Guid id){var t=await store.Find(id);return t==null?NotFound():View(t);}
 [Authorize][HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> Save(TodoModel model,string? monthDaysText){
  model.MonthDays=(monthDaysText??"").Split(',',StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries).Select(x=>int.TryParse(x,out var n)?n:0).Where(x=>x>=1&&x<=31).Distinct().Order().ToList();
  if(!TodoPalette.Colors.Contains(model.BorderColor,StringComparer.OrdinalIgnoreCase))ModelState.AddModelError(nameof(model.BorderColor),"Choose a palette color.");
  if(!new[]{"None","Daily","Weekly","Monthly","EveryNDays","MonthDays"}.Contains(model.Recurrence))ModelState.AddModelError(nameof(model.Recurrence),"Invalid recurrence.");
  if(model.Recurrence=="MonthDays"&&model.MonthDays.Count==0)ModelState.AddModelError(nameof(model.MonthDays),"Enter one or more days (1-31).");
  if(!ModelState.IsValid)return View("Edit",model);
  var old=await store.Find(model.Id);
  model.CreatedAt=old?.CreatedAt??DateTime.UtcNow;
  model.CompletedAt=model.IsFinished?(old?.CompletedAt??DateTime.UtcNow):null;
  if(old==null||old.Recurrence!=model.Recurrence||old.IntervalDays!=model.IntervalDays||!old.MonthDays.SequenceEqual(model.MonthDays))model.NextDueAt=null;else model.NextDueAt=old.NextDueAt;
  await store.Save(model);return RedirectToAction(nameof(Index));
 }
 [Authorize][HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> Toggle(Guid id){await store.Toggle(id);return RedirectToAction(nameof(Index));}
 [Authorize][HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> Delete(Guid id){await store.Delete(id);return RedirectToAction(nameof(Index));}
 [Authorize][HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> Logout(){await HttpContext.SignOutAsync();return RedirectToAction(nameof(Index));}
}

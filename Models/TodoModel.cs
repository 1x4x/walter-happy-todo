using System.ComponentModel.DataAnnotations;
namespace HappyTodo;
public class TodoModel {
 public Guid Id {get;set;}=Guid.NewGuid();
 [Required,StringLength(150)] public string Title {get;set;}="";
 [StringLength(1000)] public string Description {get;set;}="";
 public string Category {get;set;}="General";
 public bool IsFinished {get;set;}
 public DateTime? CompletedAt {get;set;}
 public DateTime CreatedAt {get;set;}=DateTime.UtcNow;
 public DateTime? NextDueAt {get;set;}
 public string Recurrence {get;set;}="None"; // None, Daily, Weekly, Monthly, EveryNDays, MonthDays
 [Range(1,365)] public int IntervalDays {get;set;}=1;
 public List<int> MonthDays {get;set;}=new();
 public string BorderColor {get;set;}="#444444";
 public bool IsImportant {get;set;}
}
public static class TodoPalette {
 public static readonly string[] Colors={"#444444","#ef4444","#f97316","#f59e0b","#eab308","#84cc16","#22c55e","#10b981","#14b8a6","#06b6d4","#0ea5e9","#3b82f6","#6366f1","#8b5cf6","#a855f7","#d946ef","#ec4899","#f43f5e","#fb7185","#fda4af"};
}

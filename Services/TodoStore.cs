using System.Text.Json;
namespace HappyTodo;
public class TodoStore {
 private readonly string path;
 private readonly SemaphoreSlim gate=new(1,1);
 private readonly JsonSerializerOptions options=new(){WriteIndented=true,PropertyNameCaseInsensitive=true};
 public TodoStore(IWebHostEnvironment env){var dir=Path.Combine(env.ContentRootPath,"App_Data");Directory.CreateDirectory(dir);path=Path.Combine(dir,"todos.json");if(!File.Exists(path)){var seed=Path.Combine(dir,"todos.seed.json");if(File.Exists(seed))File.Copy(seed,path); }}
 private async Task<List<TodoModel>> Read()=>File.Exists(path)?JsonSerializer.Deserialize<List<TodoModel>>(await File.ReadAllTextAsync(path),options)??new():new();
 private async Task Write(List<TodoModel> todos){var temp=path+"."+Guid.NewGuid()+".tmp";await File.WriteAllTextAsync(temp,JsonSerializer.Serialize(todos,options));File.Move(temp,path,true);}
 private static DateTime? NextMonthDay(DateTime from,List<int> days){for(var m=0;m<25;m++){var month=from.AddMonths(m);foreach(var d in days.Distinct().Order()){if(d<1||d>31||d>DateTime.DaysInMonth(month.Year,month.Month))continue;var candidate=new DateTime(month.Year,month.Month,d,0,0,0,DateTimeKind.Utc);if(candidate>from)return candidate;}}return null;}
 private static DateTime? NextDue(TodoModel t,DateTime after)=>t.Recurrence switch {
 "Daily"=>after.Date.AddDays(1),"Weekly"=>after.Date.AddDays(7),"Monthly"=>after.Date.AddMonths(1),"EveryNDays"=>after.Date.AddDays(Math.Clamp(t.IntervalDays,1,365)),"MonthDays"=>NextMonthDay(after,t.MonthDays),_=>null};
 private static void Refresh(TodoModel t,DateTime now){
 if(t.Recurrence=="None")return;
 if(t.NextDueAt==null){t.NextDueAt=t.Recurrence=="MonthDays"?NextMonthDay(now.AddTicks(-1),t.MonthDays):NextDue(t,now.AddDays(-1));}
 if(t.NextDueAt==null)return;
 // A completed task reopens when its next due date arrives; missed periods never pile up.
 if(t.IsFinished && t.NextDueAt<=now){t.IsFinished=false;t.CompletedAt=null;}
 if(t.NextDueAt<=now && !t.IsFinished){var next=NextDue(t,now);if(next!=null)t.NextDueAt=next;}
 }
 public async Task<List<TodoModel>> All(){await gate.WaitAsync();try{var items=await Read();var now=DateTime.UtcNow;foreach(var t in items)Refresh(t,now);await Write(items);return items.OrderBy(t=>t.IsFinished).ThenByDescending(t=>t.IsImportant).ThenBy(t=>t.NextDueAt).ThenBy(t=>t.Title).ToList();}finally{gate.Release();}}
 public async Task<TodoModel?> Find(Guid id)=>(await All()).FirstOrDefault(x=>x.Id==id);
 public async Task Save(TodoModel item){await gate.WaitAsync();try{var items=await Read();var index=items.FindIndex(x=>x.Id==item.Id);if(index<0)items.Add(item);else items[index]=item;await Write(items);}finally{gate.Release();}}
 public async Task Delete(Guid id){await gate.WaitAsync();try{var items=await Read();items.RemoveAll(x=>x.Id==id);await Write(items);}finally{gate.Release();}}
 public async Task Toggle(Guid id){await gate.WaitAsync();try{var items=await Read();var t=items.FirstOrDefault(x=>x.Id==id);if(t!=null){t.IsFinished=!t.IsFinished;t.CompletedAt=t.IsFinished?DateTime.UtcNow:null;if(t.IsFinished)t.NextDueAt=NextDue(t,DateTime.UtcNow);await Write(items);}}finally{gate.Release();}}
}

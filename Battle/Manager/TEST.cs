using System;

namespace SampleApp {
   // 委派 定義事件的形狀
   public delegate string MyDel(string str);
    
   class EventProgram {
      // 事件
      event MyDel MyEvent;
        
      // 建構子 建構後 對event註冊
      public EventProgram() {
         // 將 welcomUser function 註冊給 MyEvent event
         this.MyEvent += new MyDel(this.WelcomeUser);
      }

      public string WelcomeUser(string username) {
         return "Welcome " + username;
      }
      // 入口function
      static void Main(string[] args) {
         // new 物件 觸發建構子
         EventProgram obj1 = new EventProgram();
         // 觸發 物件的事件
         string result = obj1.MyEvent("Tutorials Point");
         Console.WriteLine(result);
      }
   }

   
}
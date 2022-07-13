using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoList.Database;

namespace ToDoList
{
    public class WorkTasksPageViewModel : BaseViewModel
    {
        public ObservableCollection<WorkTaskViewModel> WorkTaskList { get; set; } = new ObservableCollection<WorkTaskViewModel>();

        public string NewWorkTaskTitle { get; set; }
        public string NewWorkTaskDescription { get; set; }
        public ICommand AddNewTaskCommand { get; set; }
        public ICommand DeleteCompletedTasksCommand { get; set; }

        public WorkTasksPageViewModel()
        {
            NewWorkTaskTitle = string.Empty;
            NewWorkTaskDescription = string.Empty;

            AddNewTaskCommand = new RelayCommand(AddNewTask);
            DeleteCompletedTasksCommand = new RelayCommand(DeleteCompletedTasks);

            foreach (var task in DatabaseLocator.Database.WorkTasks.ToList())
            {
                WorkTaskList.Add(new WorkTaskViewModel
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    CreatedDate = task.CreatedDate,
                });
            }
        }

        private void AddNewTask()
        {
           if ((NewWorkTaskTitle != string.Empty) && (NewWorkTaskDescription != string.Empty))
            {
                var newTask = new WorkTaskViewModel
                {
                    Title = NewWorkTaskTitle,
                    Description = NewWorkTaskDescription,
                    CreatedDate = DateTime.Now,
                };

                WorkTaskList.Add(newTask);

                DatabaseLocator.Database.WorkTasks.Add(new Database.WorkTask
                {
                    Title = newTask.Title,
                    Description = newTask.Description,
                    CreatedDate = newTask.CreatedDate,

                });

                DatabaseLocator.Database.SaveChanges();

                NewWorkTaskTitle = string.Empty;
                NewWorkTaskDescription = string.Empty;

                OnPropertyChanged(nameof(NewWorkTaskTitle));
                OnPropertyChanged(nameof(NewWorkTaskDescription));
            }
            
            
        }

        private void DeleteCompletedTasks()
        {
            var completedTasks = WorkTaskList.Where(x => x.IsCompleted).ToList();

            foreach (var task in completedTasks)
            {
                WorkTaskList.Remove(task);

                var foundEntity = DatabaseLocator.Database.WorkTasks.FirstOrDefault(x => x.Id == task.Id);
                if (foundEntity != null)
                {
                    DatabaseLocator.Database.WorkTasks.Remove(foundEntity);
                }
            }
            
            DatabaseLocator.Database.SaveChanges();
        }
    }
}

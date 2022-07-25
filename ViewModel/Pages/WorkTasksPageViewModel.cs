using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ToDoList.Database;

namespace ToDoList
{

    public class WorkTasksPageViewModel : BaseViewModel
    {
        public static ObservableCollection<WorkTaskViewModel> WorkTaskList { get; set; } = new ObservableCollection<WorkTaskViewModel>();

        public ObservableCollection<WorkTaskViewModel> FilteredWorkTaskList = new ObservableCollection<WorkTaskViewModel>(from WorkTaskViewModel in WorkTaskList where WorkTaskViewModel.Id < 10 select WorkTaskViewModel);

        //public var FilteredWorkTaskLists = from WorkTaskViewModel in WorkTaskList where WorkTaskViewModel.Id < 10 select WorkTaskViewModel;

        public string NewWorkTaskTitle { get; set; }
        public string NewWorkTaskDescription { get; set; }
        public DateTime? NewDateSelected { get; set; } = DateTime.Now;
        public DateTime? SelectedDate { get; set; } = DateTime.Now;
        public ICommand AddNewTaskCommand { get; set; }
        public ICommand DeleteCompletedTasksCommand { get; set; }
        public ICommand ShowTasksCommand { get; set; }


        public WorkTasksPageViewModel()
        {
            

            NewWorkTaskTitle = string.Empty;
            NewWorkTaskDescription = string.Empty;         

            AddNewTaskCommand = new RelayCommand(AddNewTask);
            DeleteCompletedTasksCommand = new RelayCommand(DeleteCompletedTasks);
            ShowTasksCommand = new RelayCommand(ShowTasks);

            foreach (var task in DatabaseLocator.Database.WorkTasks.ToList())
            {
                WorkTaskList.Add(new WorkTaskViewModel
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    CreatedDate = task.CreatedDate,
                    dateSelected = task.dateSelected,
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
                    dateSelected = NewDateSelected,

            };

                WorkTaskList.Add(newTask);

                DatabaseLocator.Database.WorkTasks.Add(new Database.WorkTask
                {
                    Title = newTask.Title,
                    Description = newTask.Description,
                    CreatedDate = newTask.CreatedDate,
                    dateSelected = newTask.dateSelected,

                });

                DatabaseLocator.Database.SaveChanges();

                NewWorkTaskTitle = string.Empty;
                NewWorkTaskDescription = string.Empty;

                OnPropertyChanged(nameof(NewWorkTaskTitle));
                OnPropertyChanged(nameof(NewWorkTaskDescription));
                OnPropertyChanged(nameof(NewDateSelected));
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

        private void ShowTasks()
        {
            var FilteredWorkTaskList = WorkTaskList.Where(x => x.Id < 10 ).ToList();

            foreach (var task in FilteredWorkTaskList)
            {
                //WorkTaskList.Remove(task);

                //var foundEntity = DatabaseLocator.Database.WorkTasks.FirstOrDefault(x => x.Id == task.Id);
                //if (foundEntity != null)
                //{
                //    DatabaseLocator.Database.WorkTasks.Remove(foundEntity);
                //}
            }

            DatabaseLocator.Database.SaveChanges();
        }





    }
}

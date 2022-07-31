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

        public ObservableCollection<WorkTaskViewModel> FilteredWorkTaskList
        {       
            get         
            { if (conditionFiltered == true) 
              return new ObservableCollection<WorkTaskViewModel>
                  (from WorkTaskViewModel in WorkTaskList where WorkTaskViewModel.dateSelected.ToString() == SelectedDate.ToString() select WorkTaskViewModel); 
            else
              return new ObservableCollection<WorkTaskViewModel>
                  (from WorkTaskViewModel in WorkTaskList where WorkTaskViewModel.dateSelected.ToString() != SelectedDate.ToString() select WorkTaskViewModel);
            }
        }

        public string NewWorkTaskTitle { get; set; }
        public string NewWorkTaskDescription { get; set; }
        public DateTime? NewDateSelected { get; set; }
        public DateTime? SelectedDate { get; set; } = DateTime.MinValue;
        public ICommand AddNewTaskCommand { get; set; }
        public ICommand DeleteCompletedTasksCommand { get; set; }
        public ICommand EditTasksCommand { get; set; }
        public ICommand ShowTasksCommand { get; set; }
        public ICommand ShowAllTasksCommand { get; set; }
        public bool conditionFiltered { get; set; } = false;

        public WorkTasksPageViewModel()
        {

           NewWorkTaskTitle = string.Empty;
           NewWorkTaskDescription = string.Empty;
        
            AddNewTaskCommand = new RelayCommand(AddNewTask);
            DeleteCompletedTasksCommand = new RelayCommand(DeleteCompletedTasks);
            EditTasksCommand = new RelayCommand(EditTasks);
            ShowTasksCommand = new RelayCommand(ShowTasks);
            ShowAllTasksCommand = new RelayCommand(ShowAllTasks);

            foreach (var task in DatabaseLocator.Database.WorkTasks.ToList())
            {
                WorkTaskList.Add(new WorkTaskViewModel
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    dateSelected = task.dateSelected,
                });
            }
            OnPropertyChanged(nameof(FilteredWorkTaskList));
            OnPropertyChanged(nameof(NewDateSelected));
        }

        private void AddNewTask()
        {
           if ((NewWorkTaskTitle != string.Empty) && (NewWorkTaskDescription != string.Empty) && (NewDateSelected != null))
            {
                var newTask = new WorkTaskViewModel
                {
                    Title = NewWorkTaskTitle,
                    Description = NewWorkTaskDescription,
                    dateSelected = NewDateSelected,

                };

                WorkTaskList.Add(newTask);

                DatabaseLocator.Database.WorkTasks.Add(new Database.WorkTask
                {
                    Title = newTask.Title,
                    Description = newTask.Description,
                    dateSelected = newTask.dateSelected,

                });

                DatabaseLocator.Database.SaveChanges();

                NewWorkTaskTitle = string.Empty;
                NewWorkTaskDescription = string.Empty;
                NewDateSelected = null;

                OnPropertyChanged(nameof(NewWorkTaskTitle));
                OnPropertyChanged(nameof(NewWorkTaskDescription));
                OnPropertyChanged(nameof(NewDateSelected));
                OnPropertyChanged(nameof(FilteredWorkTaskList));
                
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
            OnPropertyChanged(nameof(FilteredWorkTaskList));

            DatabaseLocator.Database.SaveChanges();   
        }

        private void ShowTasks()
        {
            conditionFiltered = true;
           
            OnPropertyChanged(nameof(FilteredWorkTaskList));
        }

        public void ShowAllTasks()
        {
            conditionFiltered = false;
            SelectedDate = DateTime.MinValue;
            OnPropertyChanged(nameof(FilteredWorkTaskList));
        }

        private void EditTasks()
        {

            var editTasks = WorkTaskList.Where(x => x.IsCompleted);

            foreach (var task in editTasks)
            {

                var foundEntity = DatabaseLocator.Database.WorkTasks.FirstOrDefault(x => x.Id == task.Id);
                if (foundEntity != null)
                {
                    foundEntity.Id = foundEntity.Id;
                    foundEntity.Title = NewWorkTaskTitle;
                    foundEntity.Description = NewWorkTaskDescription;
                    foundEntity.dateSelected = NewDateSelected;
                 
                    //DatabaseLocator.Database.WorkTasks.Update(foundEntity);
                }
                
            }

           OnPropertyChanged(nameof(FilteredWorkTaskList));

           DatabaseLocator.Database.SaveChanges();

        }


    }
}

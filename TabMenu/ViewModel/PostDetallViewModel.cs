using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using BancDelTemps.Model;
using BancDelTemps.Model.Class;
using BancDelTemps.View;

namespace BancDelTemps.ViewModel
{
    public class PostDetallViewModel : INotifyPropertyChanged
    {
        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
        #region RelayCommand
        class RelayCommand : ICommand
        {
            private Action<object> _action;

            public RelayCommand(Action<object> action)
            {
                _action = action;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (parameter != null)
                {
                    _action(parameter);
                }
                else
                {
                    _action("Hello world");
                }
            }

            public event EventHandler CanExecuteChanged;
        }
        #endregion

        public ICommand ButtonCloseApp { get; set; }
        public ICommand ButtonSaveChanges { get; set; }
        public ICommand ButtonDiscardChanges { get; set; }
        public ICommand ButtonMostrarDetallsUsuari { get; set; }
        public ICommand ButtonDeletePost { get; set; }
        public Post Post { get; set; }

        public PostDetallViewModel()
        {
        }

        public PostDetallViewModel(Post post)
        {
            ButtonCloseApp = new PostDetallViewModel.RelayCommand(o => Application.Current.Windows[1].Close());
            ButtonSaveChanges = new PostDetallViewModel.RelayCommand(o => saveChanges());
            ButtonDiscardChanges = new PostDetallViewModel.RelayCommand(o => discardChanges());
            ButtonMostrarDetallsUsuari = new PostDetallViewModel.RelayCommand(o => userDetails());
            ButtonDeletePost = new PostDetallViewModel.RelayCommand(o => deletePost());
            Categories = CategoriesRepository.GetAllCategoriyNames();
            Post = post;
            AuthorPost = (post.User.name + " " + post.User.last_name);
            DescriptionPost = post.description;
            TitlePost = post.title;
            LocationPost = post.location;
            IdPost = post.Id_Post;
            SelectedItemCategory = CategoriesRepository.GetSingleCategory(post.Category_Id_Category).name;
            DateCreatedPost = DateTime.Parse(post.date_created);
            if(post.date_finished != null) DateFinishedPost = DateTime.Parse(post.date_finished);

        }

        public void saveChanges()
        {
            Post pNew = new Post();
            pNew.title = TitlePost;
            pNew.description = DescriptionPost;
            pNew.Id_Post = IdPost;
            pNew.date_created = DateCreatedPost.ToString("dd-MM-yyyy");
            pNew.date_finished = DateFinishedPost.ToString("dd-MM-yyyy");
            pNew.location = LocationPost;
            pNew.Category_Id_Category = CategoriesRepository.GetCategoryIdByString(SelectedItemCategory); 
            pNew.actiu = Post.actiu;
            pNew.UserId_User = Post.UserId_User;
            PostsRepository.UpdatePost(pNew);
            Application.Current.Windows[1].Close();
        }

        public void discardChanges()
        {
            MessageBoxResult mbRes = MessageBox.Show("Are you sure you want to discard changes?", "Warning", MessageBoxButton.YesNo);
            if (mbRes == MessageBoxResult.Yes)
            {
                Application.Current.Windows[1].Close();
            }
        }

        public void deletePost()
        {
            MessageBoxResult mbRes = MessageBox.Show("Are you sure you want to delete this post?", "Warning", MessageBoxButton.YesNo);
            if (mbRes == MessageBoxResult.Yes)
            {
                PostsRepository.DeletePost(Post.Id_Post);
                Application.Current.Windows[1].Close();
            }
        }

        public void userDetails()
        {
                Informacio i = new Informacio(Post.User);
                i.ShowDialog();
        }

        private int _idPost;
        public int IdPost
        {
            get { return _idPost; }
            set
            {
                _idPost = value; NotifyPropertyChanged();
            }
        }

        private List<String> _categories;
        public List<String> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value; NotifyPropertyChanged();
            }
        }

        private string _selectedItemCategory;
        public string SelectedItemCategory
        {
            get { return _selectedItemCategory; }
            set
            {
                _selectedItemCategory = value; NotifyPropertyChanged();
            }
        }

        private string _authorPost;
        public string AuthorPost
        {
            get { return _authorPost; }
            set
            {
                _authorPost = value; NotifyPropertyChanged();
            }
        }

        private string _categoryPost;
        public string CategoryPost
        {
            get { return _categoryPost; }
            set
            {
                _categoryPost = value; NotifyPropertyChanged();
            }
        }

        private string _descriptionPost;
        public string DescriptionPost
        {
            get { return _descriptionPost; }
            set
            {
                _descriptionPost = value; NotifyPropertyChanged();
            }
        }

        private string _titlePost;
        public string TitlePost
        {
            get { return _titlePost; }
            set
            {
                _titlePost = value; NotifyPropertyChanged();
            }
        }

        private string _locationPost;
        public string LocationPost
        {
            get { return _locationPost; }
            set
            {
                _locationPost = value; NotifyPropertyChanged();
            }
        }

        private DateTime _dateCreatedPost;
        public DateTime DateCreatedPost
        {
            get { return _dateCreatedPost; }
            set
            {
                _dateCreatedPost = value; NotifyPropertyChanged();
            }
        }

        private DateTime _dateFinishedPost;
        public DateTime DateFinishedPost
        {
            get { return _dateFinishedPost; }
            set
            {
                _dateFinishedPost = value; NotifyPropertyChanged();
            }
        }
    }
}
﻿using MicrosfferToDo.Library.Commum;
using MicrosfferToDo.Library.Util;
using MicrosfferToDo.WPF.Model;
using MicrosfferToDo.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrosfferToDo.WPF.Command
{
    public class DeletarAtividadeCommand : ICommand
    {

        #region " ### campos " 

        private ToDoViewModel _todoViewModel;

        #endregion

        #region "#### Construtor " 

        public DeletarAtividadeCommand(ToDoViewModel _viewModel)
        {
            _todoViewModel = _viewModel;
        }
        #endregion

        #region ICommand Members

        /// <summary>
        /// 
        /// </summary>
        public bool CanExecute(object parameter)
        {
            if (!string.IsNullOrEmpty(_todoViewModel.NomeTodo))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Actions to take when CanExecute() changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Inclui um novo clube ou altera um existente.
        /// </summary>
        public void Execute(object parameter)
        {
            var client = HttpClientRequest.getClient();

            HttpResponseMessage response = client.DeleteAsync(EnderecosWebAPI._delete+ _todoViewModel.Atividade.IdTodo).Result;
            Uri envioUri = response.Headers.Location;

            if (response.IsSuccessStatusCode)
            {
                CarregarAtividadeCommand _carr = new CarregarAtividadeCommand(_todoViewModel);
                _carr.Execute(null);

                _todoViewModel.IdTodo = 0;
                _todoViewModel.NomeTodo = string.Empty;
            }
        }
        #endregion
    }
}

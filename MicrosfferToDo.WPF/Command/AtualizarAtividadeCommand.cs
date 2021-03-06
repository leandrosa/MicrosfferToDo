﻿using MicrosfferToDo.Library.Util;
using MicrosfferToDo.WPF.Model;
using MicrosfferToDo.WPF.ViewModel;
using System;
using System.Net.Http;
using System.Windows.Input;
using MicrosfferToDo.Library.Constantes;
using static System.String;

namespace MicrosfferToDo.WPF.Command
{
    public class AtualizarAtividadeCommand : ICommand
    {
        public ToDoViewModel ViewModel { get; }


        ///<summary>
        /// Recebe os objetos da ViewModel e atribui para a propriedade privada da classe
        /// </summary>
        public AtualizarAtividadeCommand(ToDoViewModel viewModel)
        {
            ViewModel = viewModel;
        }
       

        
        /// <summary>
        /// Verifica a propriedade para habilitar o botão ou não
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return IsNullOrEmpty(ViewModel.NomeTodo);
        }

        /// <summary>
        /// Ação que verifica se pode executar CanExecute(object).
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Executa a alteração dos dados de acordo com o que foi recebido da View
        /// Chama a Web API passando os parâmetros
        /// </summary>
        public void Execute(object parameter)
        {
            //Chama a classe Request
            //Dentro da classe tem o Token e a Senha
            var client = HttpClientRequest.GetClient();

            //preenche os dados da classe para passar para a WebAPI
            var atividades = new AtividadeToDo()
            {
                NomeTodo = ViewModel.NomeTodo,
                CompletoTodo = 0,
                IdTodo = ViewModel.IdTodo
            };

            //Chama a classe PUT passando a constante, id e os dados
            HttpResponseMessage response = client.PutAsJsonAsync(EnderecosWebApi.Put + ViewModel.IdTodo, atividades).Result;

            //Se a resposta do Web Api retornar com sucesso, carrega os dados novamente
            if (response.IsSuccessStatusCode)
            {
                CarregarAtividadeCommand carregaAtividadeCommand = new CarregarAtividadeCommand(ViewModel);
                carregaAtividadeCommand.Execute(null);

                CancelarEdicaoCommand cancelaEdicaoCommand = new CancelarEdicaoCommand(ViewModel);
                cancelaEdicaoCommand.Execute(null);
            }
        }
       
    }
}


import { Component, OnInit } from '@angular/core';
import { EventoService } from '../services/evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any = [];//[] Indica que agora o objeto eventos será um array vazio, dando assim propriedades 
  //e formas de adicionar e remover objetos do array
  imgWidth: number = 150;
  imgMargin: number = 2;
  isCollapsed : boolean = true;
  listFilter: string = '';
  constructor(private eventoService: EventoService) { }

  /* O ngOnInit é um método do ciclo de vida do componente no Angular que é chamado automaticamente 
  após a inicialização do componente. É usado para realizar inicializações ou configurações 
  necessárias, como buscar dados de serviços, configurar variáveis ou assinar eventos. 
  É recomendado para tarefas que precisam ser executadas apenas uma vez durante o ciclo de vida 
  do componente.*/
  ngOnInit(): void {
    this.getEventos();
  }

  collapseImage(){
    // Essa função é responsável por alternar o estado do atributo isCollapsed que é usado para controle da visibilidade do componente child app-imagemEvento.
    // O valor de isCollapsed é inicialmente definido como true, logo, ao chamar essa função, o valor de isCollapsed será alterado para false, 
    // causando a visibilidade do componente child app-imagemEvento ser revelado.
    // Caso o valor de isCollapsed seja false, ao chamar essa função, o valor de isCollapsed será alterado para true, 
    // causando a visibilidade do componente child app-imagemEvento ser ocultado.
    this.isCollapsed = !this.isCollapsed;}

  public getEventos(): void {
    // Requisitando todos os eventos do backend (API)
    // O método subscribe() é chamado automaticamente quando a requisição HTTP para a API estiver completa
    // Se a requisição for bem sucedida, o método response é chamado com os dados de retorno da API
    // Se a requisição não for bem sucedida, o método error é chamado com a mensagem de erro da resposta HTTP
    this.eventoService.getEvento()
      .subscribe(
        response => { // Dados de retorno da requisição à API
          this.eventos = response; // Atribuindo os dados de retorno da API à variável 'eventos' do componente
        },
        error => console.log('Erro ao requisitar eventos:', error) // Imprimindo a mensagem de erro no console
      )

  }
}

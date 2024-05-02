
import { Component, OnInit } from '@angular/core';
import { EventoService } from '../services/evento.service';
import { Evento } from '../models/Evento';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
  //providers: [EventoService] //outro metodo para injetar o service
})
export class EventosComponent implements OnInit {

  public eventos: any = [];//[] Indica que agora o objeto eventos será um array vazio, dando assim propriedades
  //e formas de adicionar e remover objetos do array
  public eventosFiltrados: any = [];
  imgWidth: number = 150;
  imgMargin: number = 2;
  isCollapsed : boolean = true;
  private _filtroLista: string = '';

  public get filtroLista() : string {
    return this._filtroLista;

  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  filtrarEventos(filtrarPor: string): any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
       evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

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
    this.eventoService.getEventos()
      .subscribe(
        (_eventos: Evento[]) => { // Dados de retorno da requisição à API
          this.eventos = _eventos; // Atribuindo os dados de retorno da API à variável 'eventos' do componente
this.eventosFiltrados = _eventos;
        },
        error => console.log('Erro ao requisitar eventos:', error) // Imprimindo a mensagem de erro no console
      )

  }
}

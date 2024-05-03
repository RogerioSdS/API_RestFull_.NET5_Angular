import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../services/evento.service';
import { Evento } from '../models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
  //providers: [EventoService] //outro metodo para injetar o service
})
export class EventosComponent implements OnInit {
  modalRef?: BsModalRef;
  public eventos: Evento[] = []; //[] Indica que agora o objeto eventos será um array vazio, dando assim propriedades
  //e formas de adicionar e remover objetos do array
  public eventosFiltrados: Evento[] = [];
  public imgWidth = 150;
  public imgMargin = 2;
  public isCollapsed = true;
  private filtroListado: string = '';

  public get filtroLista(): string {
    return this.filtroListado;
  }

  public set filtroLista(value: string) {
    this.filtroListado = value;
    this.eventosFiltrados = this.filtroLista
      ? this.filtrarEventos(this.filtroLista)
      : this.eventos;
  }

  public filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: any) =>
        evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService
  ) {}

  /* O ngOnInit é um método do ciclo de vida do componente no Angular que é chamado automaticamente
  após a inicialização do componente. É usado para realizar inicializações ou configurações
  necessárias, como buscar dados de serviços, configurar variáveis ou assinar eventos.
  É recomendado para tarefas que precisam ser executadas apenas uma vez durante o ciclo de vida
  do componente.*/
  public ngOnInit(): void {
    this.getEventos();
  }

  public collapseImage() {
    // Essa função é responsável por alternar o estado do atributo isCollapsed que é usado para controle da visibilidade do componente child app-imagemEvento.
    // O valor de isCollapsed é inicialmente definido como true, logo, ao chamar essa função, o valor de isCollapsed será alterado para false,
    // causando a visibilidade do componente child app-imagemEvento ser revelado.
    // Caso o valor de isCollapsed seja false, ao chamar essa função, o valor de isCollapsed será alterado para true,
    // causando a visibilidade do componente child app-imagemEvento ser ocultado.
    this.isCollapsed = !this.isCollapsed;
  }

  public getEventos(): void {
    // Requisitando todos os eventos do backend (API)
    // O método subscribe() é chamado automaticamente quando a requisição HTTP para a API estiver completa
    // Se a requisição for bem sucedida, o método response é chamado com os dados de retorno da API
    // Se a requisição não for bem sucedida, o método error é chamado com a mensagem de erro da resposta HTTP
    const observer = {
      next: (eventos: Evento[]) => {
        this.eventos = eventos;
        this.eventosFiltrados = eventos;
      },
      error: (error : any) => console.log('Erro ao requisitar eventos:', error),
    };
    //Foi atualizado o antigo codigo, pois o mesmo fazia a inscrição no observable e chamava o subscribe.
    this.eventoService.getEventos().subscribe(/*se increvendo no observable*/
      observer);
      /*
      this.eventoService.getEventos().subscribe(
      (eventosRest: Evento[]) => {
        // Dados de retorno da requisição à API
        this.eventos = eventosRest; // Atribuindo os dados de retorno da API à variável 'eventos' do componente
        this.eventosFiltrados = eventosRest;
      }, /*quando temos uma aplicação que utiliza o observable, ela só encerra o processo de observable quando finaliza o processo
      ou quando vc se desinscreve do observable*
      (error) => console.log('Erro ao requisitar eventos:', error) // Imprimindo a mensagem de erro no console*/

  }

  openModal(template: TemplateRef<void>): void{
    this.modalRef = this.modalService.show(template);
  }

  confirm(): void {
    this.modalRef?.hide();
  }

  decline(): void {
    this.modalRef?.hide();

  }

}

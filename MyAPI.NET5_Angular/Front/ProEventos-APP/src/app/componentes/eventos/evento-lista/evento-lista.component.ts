import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss'],
})
export class EventoListaComponent implements OnInit {
  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public eventoId = 0;
  public pagination = {} as Pagination;

  public imgWidth = 150;
  public imgMargin = 2;
  public isCollapsed = true;
  private filtroListado: string = '';

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  // Declara e inicializa um Subject chamado 'termoBuscaChanged' do tipo 'string'.
  // O Subject é um tipo especial de Observable que permite emitir valores e
  // também permite que outros objetos se inscrevam para receber esses valores.
  // Neste caso, 'termoBuscaChanged' será usado para transmitir os valores do termo de busca.
  termoBuscaChanged: Subject<string> = new Subject<string>();

  public filtrarEventos(evt: any): void {
    // Se não há observadores inscritos em 'termoBuscaChanged'
    if (this.termoBuscaChanged.observers.length === 0) {
      // Configura o observable para usar debounceTime e se inscreve
      this.termoBuscaChanged
        .pipe(debounceTime(1000)) // Atraso de 1 segundo após a última emissão
        .subscribe((filtrarPor) => {
          // Mostra o spinner/loader
          this.spinner.show();
          // Chama o serviço para buscar eventos filtrados
          this.eventoService
            .getEventos(
              this.pagination.currentPage, // Página atual
              this.pagination.itemsPerPage, // Itens por página
              filtrarPor // Termo de busca
            )
            // Inscreve-se no resultado da busca
            .subscribe(
              (paginatedResult: PaginatedResult<Evento[]>) => {
                // Atualiza a lista de eventos e paginação
                this.eventos = paginatedResult.result;
                this.pagination = paginatedResult.pagination;
              },
              (error: any) => {
                // Esconde o spinner e mostra mensagem de erro
                this.spinner.hide();
                this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
              }
            )
            // Esconde o spinner após a conclusão (sucesso ou erro)
            .add(() => this.spinner.hide());
        });
    }
    // Emite o valor atual do campo de busca ('evt.value') para o Subject 'termoBuscaChanged'.
    // Isso aciona o fluxo configurado com debounce, que aguardará 1 segundo após a última emissão
    // antes de executar a lógica de busca, evitando chamadas excessivas ao serviço.
    this.termoBuscaChanged.next(evt.value);
  }

  public ngOnInit(): void {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 3,
      totalItems: 1,
    } as Pagination;

    this.carregarEventos();
  }

  public collapseImage() {
    this.isCollapsed = !this.isCollapsed;
  }

  public mostrarImagem(imagemURL: string): string {
    return imagemURL !== ''
      ? `${environment.apiURL}resources/images/${imagemURL}`
      : 'assets/img/sem_imagem.jpg';
  }

  public carregarEventos(): void {
    this.spinner.show();

    this.eventoService
      .getEventos(this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe(
        (paginatedResult: PaginatedResult<Evento[]>) => {
          this.eventos = paginatedResult.result;
          this.pagination = paginatedResult.pagination;
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
        }
      )
      .add(() => this.spinner.hide());
  }

  public pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.carregarEventos();
  }

  openModal(event: any, template: TemplateRef<void>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template);
  }

  confirm(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.eventoService
      .deleteEvento(this.eventoId)
      .subscribe(
        (result: any) => {
          if (result.message === 'Deletado') {
            this.toastr.success(
              'O evento foi deletado com sucesso',
              'Deletado'
            );
            this.carregarEventos();
          }
        },
        (error: any) => {
          this.toastr.error(
            `Erro ao tentar deletar o evento: ${this.eventoId}`,
            'Erro'
          );
        }
      )
      .add(() => this.spinner.hide());
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }
}

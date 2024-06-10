import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss'],
})
export class EventoListaComponent implements OnInit {
  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public eventoId = 0;

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
      (evento: Evento) =>
        evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  public ngOnInit(): void {
    this.spinner.show();
    this.carregarEventos();
  }

  public collapseImage() {
    this.isCollapsed = !this.isCollapsed;
  }

  public carregarEventos(): void {
    const observer = {
      next: (eventos: Evento[]) => {
        if (eventos) {
          //console.log('eventos\n' + JSON.stringify(eventos));
          // Mapeia cada evento para garantir que a propriedade 'lotes' seja sempre um array (pode ser vazio), evitando 'null' ou 'undefined'
          this.eventos = eventos.map((evento) => ({
            ...evento, // Copia todas as propriedades do evento original
            lotes: evento.lotes ? evento.lotes : [], // Garante que 'lotes' seja um array, mesmo se for 'null' ou 'undefined'
          }));
          this.eventosFiltrados = this.eventos;
        }
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos', 'Erro!');
      },
      complete: () => this.spinner.hide(),
    };

    this.eventoService.getEventos().subscribe(observer);
  }

  openModal(event: any, template: TemplateRef<void>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template);
  }

  confirm(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        if (result.message === 'Deletado') {
          this.toastr.success('O evento foi deletado com sucesso', 'Deletado');
          this.carregarEventos();
        }
      },
      (error: any) => {
        this.toastr.error(
          `Erro ao tentar deletar o evento: ${this.eventoId}`,
          'Erro'
        );
      },
      () => this.spinner.hide()
    );
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }
}

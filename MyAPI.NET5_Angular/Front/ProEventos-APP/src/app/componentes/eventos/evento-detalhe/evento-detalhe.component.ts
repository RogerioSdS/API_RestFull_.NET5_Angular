import { Component, OnInit, TemplateRef } from '@angular/core';
import {
  AbstractControl,
  Form,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {

  modalRef : BsModalRef;
  eventoId : number;
  evento = {} as Evento;
  form: FormGroup;
  estadoSalvar = 'post';
  loteAtual = {id:0, nome: '', indice:0};

  get modoEditar(): boolean {
    return this.estadoSalvar === 'put';
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }
  get f(): any {
    /*quando for utilizar o form, o f é o nome do form, ele ira retornar os controles do form que foram inserido no metodo validation */
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false,
    };
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private modalService: BsModalService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,
    private eventoService: EventoService,
    private loteService: LoteService,
  ) {
    this.localeService.use('pt-BR');
  }

  public carregarEvento(): void {
    //console.log("estadoSalvar ="+this.estadoSalvar);
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');
    //console.log("eventoID ="+this.eventoId);

    if (this.eventoId !== null && this.eventoId > 0) {
      this.estadoSalvar = 'put';
      //console.log("estadoSalvar_Atualizado ="+this.estadoSalvar);
      this.spinner.show();
      this.eventoService.getEventoById(this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
          this.evento.lotes.forEach((lote) => {
            this.lotes.push(this.criarLote(lote));
          })
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error(`Erro ao tentar carregar o evento`);
          console.error(error);
        },
        () => {
          this.spinner.hide();
        }
      );
    }
  }

  ngOnInit(): void {
    this.carregarEvento();
    this.validation();
  }

  public validation(): void {
    /**
     * Este método cria um formulário de validação para um evento. Ele recebe o
     * formBuilder como parâmetro e retorna um objeto do tipo FormGroup. O
     * formGroup é um objeto que contém os campos do formulário e as respectivas
     * validações.
     */
    this.form = this.fb.group({
      // Tema do evento (obrigatório, mínimo 4 caracteres, máximo 50)
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],

      // Data do evento (obrigatório)
      dataEvento: ['', Validators.required],

      // Local do evento (obrigatório)
      local: ['', Validators.required],

      // Quantidade de pessoas (obrigatório, máximo 120.000)
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],

      // Telefone (obrigatório)
      telefone: ['', Validators.required],

      // E-mail (obrigatório, deve ser um e-mail válido)
      email: ['', [Validators.required, Validators.email]],

      // URL da imagem do evento (obrigatório)
      imagemURL: ['', Validators.required],
      lotes: this.fb.array([]),
    });
  }

  adicionarLote(): void{
    this.lotes.push(
      this.criarLote({id:0} as Lote)
    );
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    })
  }

  mudarValorData(value: Date, indice: number, campo: string):void{
    this.lotes.value[indice][campo] = value;

    if(this.lotes.value[indice].dataFim < value){
      this.lotes.value[indice].dataFim = value;
    }
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoFormat: FormControl | AbstractControl): any {
    return { 'is-invalid': campoFormat.invalid && campoFormat.touched };
  }

  salvarEvento(): void {
    this.spinner.show();
    if (this.form.valid) {

     this.evento = this.estadoSalvar === 'post'
        ?  {...this.form.value}
        : {eventoId: this.evento.eventoId, ...this.form.value };

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        //aqui seria o mesmo que fazer isso:
        //chamar o evento service, que é a controller do evento
        //passar o metodo put ou post, depende da situação
        //e passar o obj. Evento como parametro
        (eventoRetorno: Evento) => {
          this.toastr.success('O evento foi salvo com sucesso!', 'Sucesso!');
          this.router.navigate([`eventos/detalhe/${eventoRetorno.eventoId}`]);
        },
        (error: any) => {
          console.error(error);
          this.toastr.error(`Erro ao tentar salvar o evento: ${error}`);
        },
      )
      .add(() => this.spinner.hide());
    }
  }

  public salvarLotes() : void{
    if (this.form.controls.lotes.valid){
      this.spinner.show();
      this.loteService.saveLote(this.eventoId, this.form.value.lotes)
      .subscribe(
        () =>{
          this.toastr.success('Lotes salvos com sucesso', 'Sucesso');
          // Reset do form array 'lotes' após o envio de todos os lotes.
          // O reset() é usado para limpar o form array, removendo todos os lotes adicionados.
          // Após o envio de todos os lotes, é necessário resetar o form array para que o usuário
          // possa adicionar novos lotes.

          //this.lotes.reset();
        },
        (error: any) =>{
          this.toastr.error(`Erro ao tentar salvar lotes`,'Erro');
          console.error(error);
        }
      ).add(()=> this.spinner.hide());
    }
  }

  public removerLote(template: TemplateRef<any>, indice: number): void{
    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirmDeleteLote(): void{
    this.modalRef.hide();
    this.spinner.show();
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () => {
        this.toastr.success('Lote deletado com sucesso', 'Sucesso');
        this.modalRef.hide();
        this.lotes.removeAt(this.loteAtual.indice);
      },
      (error: any) => {
        this.toastr.error(`Erro ao tentar deletar o lote: ${this.loteAtual.id}`, 'Erro');
        console.error(error);
      }
    ).add(()=> this.spinner.hide());
  }

  declineDeleteLote(): void{
    this.modalRef.hide();
  }

}

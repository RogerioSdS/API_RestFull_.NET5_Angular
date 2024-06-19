import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  Form,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  evento = {} as Evento;
  form: FormGroup;
  estadoSalvar = 'post';

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
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {
    this.localeService.use('pt-BR');
  }

  public carregarEvento(): void {
    console.log("estadoSalvar ="+this.estadoSalvar);
    const eventoIdParam = +this.router.snapshot.paramMap.get('id');
    console.log("eventoID ="+eventoIdParam);

    if (eventoIdParam !== null && eventoIdParam > 0) {
      this.estadoSalvar = 'put';
      console.log("estadoSalvar_Atualizado ="+this.estadoSalvar);
      this.spinner.show();
      this.eventoService.getEventoById(+eventoIdParam).subscribe(
        (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
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
      quantidade: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    })
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoFormat: FormControl | AbstractControl): any {
    return { 'is-invalid': campoFormat.invalid && campoFormat.touched };
  }

  salvarAteracao(): void {
    this.spinner.show();
    if (this.form.valid) {

     this.evento = this.estadoSalvar === 'post'
        ?  {...this.form.value}
        : {eventoId: this.evento.eventoId, ...this.form.value };

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        () => {
          this.toastr.success('O evento foi salvo com sucesso!', 'Sucesso!');
        },
        (error: any) => {
          console.error(error);
          this.toastr.error(`Erro ao tentar salvar o evento: ${error}`);
        },
      )
      .add(() => this.spinner.hide());
    }
  }
}

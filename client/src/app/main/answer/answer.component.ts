import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { MessageService, ConfirmationService } from 'primeng/api';
import { first } from 'rxjs/operators';
import { AnswerService } from 'src/app/_services/answer.service';

@Component({
  selector: 'app-answer',
  templateUrl: './answer.component.html',
  styleUrls: ['./answer.component.css']
})
export class AnswerComponent implements OnInit {
  questionId: number;
  surveyId: number;
  displayEdit: boolean = false;
  displayAdd: boolean = false;
  formAdd: FormGroup;
  formEdit: FormGroup;
  answerTypes: any;
  answers: any;
  id_Edit = 0;
  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private answerService: AnswerService,
    private fb: FormBuilder,
    private spinner: NgxSpinnerService,
    private activatedRoute: ActivatedRoute
  ) { }
  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params: Params) => {
      this.questionId = params["questionId"];
      this.surveyId = params["surveyId"];
      this.loadData();
    });

    this.formAdd = this.fb.group({
      Content: this.fb.control('', [Validators.required]),
      QuestionId: this.fb.control(this.questionId, [Validators.required]),
    });
    this.formEdit = this.fb.group({
      Content: this.fb.control('', [Validators.required]),
    });
  }
  loadData(): void {
    this.spinner.show();
    this.answerService
      .getByQuestionId(this.questionId)
      .pipe(first())
      .subscribe({
        next: (res) => {
          this.answers = res;
          this.spinner.hide();
        },
        error: (err) => {
          this.spinner.hide();
          if (err.StatusCode) {
            this.messageService.add({
              severity: 'error',
              summary: err.Message,
              detail: err.Errors,
            });
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'Th??ng b??o',
              detail: `???? c?? l???i !`,
            });
          }
        },
      });
  }

  onAdd(): void {
    //console.log(this.formAdd.value);
    this.answerService
      .add(this.formAdd.value)
      .pipe(first())
      .subscribe({
        next: (res: any) => {
          if (res !== null) {
            this.messageService.add({
              severity: 'success',
              summary: 'Th??ng b??o',
              detail: 'Th??m th??nh c??ng !',
            });
            this.displayAdd = false;
            this.clearModalAdd();
            this.loadData();
          }
        },
        error: (err: any) => {
          if (err.StatusCode) {
            this.messageService.add({
              severity: 'error',
              summary: err.Message,
              detail: err.Errors,
            });
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'Th??ng b??o',
              detail: `???? c?? l???i !`,
            });
          }
        },
      });
  }

  onGetEdit(id: any): void {
    this.spinner.show();
    this.answerService
      .getById(id)
      .pipe(first())
      .subscribe({
        next: (res) => {
          this.displayEdit = true;
          this.id_Edit = res.Id;
          this.formEdit = this.fb.group({
            Content: this.fb.control(res.Content, [Validators.required]),
            QuestionId: this.fb.control(res.QuestionId, [Validators.required]),
          });
          this.spinner.hide();
        },
        error: (err) => {
          if (err.StatusCode) {
            this.messageService.add({
              severity: 'error',
              summary: err.Message,
              detail: err.Errors,
            });
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'Th??ng b??o',
              detail: `???? c?? l???i !`,
            });
          }
          this.spinner.hide();
        },
      });
  }
  onEdit(): void {
    if (this.id_Edit > 0) {
      this.answerService
        .update(this.id_Edit, this.formEdit.value)
        .pipe(first())
        .subscribe({
          next: (res) => {
            if (res !== null) {
              this.messageService.add({
                severity: 'success',
                summary: 'Th??ng b??o',
                detail: 'C???p nh???t th??nh c??ng !',
              });
              this.displayEdit = false;
              this.loadData();
            }
          },
          error: (err) => {
            if (err.StatusCode) {
              this.messageService.add({
                severity: 'error',
                summary: err.Message,
                detail: err.Errors,
              });
            } else {
              this.messageService.add({
                severity: 'error',
                summary: 'Th??ng b??o',
                detail: `???? c?? l???i !`,
              });
            }
          },
        });
    }
  }
  clearModalAdd() {
    this.formAdd = this.fb.group({
      Content: this.fb.control('', [Validators.required]),
      QuestionId: this.fb.control(this.questionId, [Validators.required]),
    });
  }
  onDelete(id: any) {
    this.confirmationService.confirm({
      header: 'Xo?? kh???o s??t ?',
      message: 'B???n c?? ch???c ch???n xo?? ?',
      accept: () => {
        this.answerService
          .delete(id)
          .pipe(first())
          .subscribe({
            next: (res) => {
              //console.log(res);
              if (res !== null) {
                this.messageService.add({
                  severity: 'success',
                  summary: 'Th??ng b??o',
                  detail: '???? xo?? th??nh c??ng !',
                });
                this.loadData();
              }
            },
            error: (err) => {
              if (err.StatusCode) {
                this.messageService.add({
                  severity: 'error',
                  summary: err.Message,
                  detail: err.Errors,
                });
              } else {
                this.messageService.add({
                  severity: 'error',
                  summary: 'Th??ng b??o',
                  detail: `???? c?? l???i !`,
                });
              }
            },
          });
      },
    });
  }

}

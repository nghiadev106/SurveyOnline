import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { MessageService, ConfirmationService } from 'primeng/api';
import { first } from 'rxjs/operators';
import { SurveyService } from 'src/app/_services/survey.service';

@Component({
  selector: 'app-survey-detail',
  templateUrl: './survey-detail.component.html',
  styleUrls: ['./survey-detail.component.css']
})
export class SurveyDetailComponent implements OnInit {

  surveyId: number;
  survey: any;
  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private surveyService: SurveyService,
    private fb: FormBuilder,
    private spinner: NgxSpinnerService,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params: Params) => {
      this.surveyId = params["surveyId"];
      this.loadData();
    });
  }

  loadData(): void {
    this.spinner.show();
    this.surveyService
      .GetSurveyDetail(this.surveyId)
      .pipe(first())
      .subscribe({
        next: (res) => {
          this.survey = res;
          console.log(res);
          this.spinner.hide();
        },
        error: (err) => {
          this.spinner.hide();
          this.messageService.add({
            severity: 'error',
            summary: 'Thông báo',
            detail: `Đã có lỗi !`,
          });
        },
      });
  }

}

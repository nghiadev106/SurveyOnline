import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { MessageService } from 'primeng/api';
import { first } from 'rxjs/operators';
import { SurveyService } from 'src/app/_services/survey.service';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {

  surveyId: number;
  questions: any;
  count: any;
  constructor(
    private messageService: MessageService,
    private surveyService: SurveyService,
    private spinner: NgxSpinnerService,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params: Params) => {
      this.surveyId = params["surveyId"];
      this.getUserStatistics();
      this.getRatioStatistics();
    });
  }

  getUserStatistics(): void {
    this.spinner.show();
    this.surveyService
      .getUserStatistics(this.surveyId)
      .pipe(first())
      .subscribe({
        next: (res) => {
          this.count = res;
          console.log(res);
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
              summary: 'Thông báo',
              detail: `Đã có lỗi !`,
            });
          }
        },
      });
  }

  getRatioStatistics(): void {
    this.spinner.show();
    this.surveyService
      .getRatioStatistics(this.surveyId)
      .pipe(first())
      .subscribe({
        next: (res) => {
          this.questions = res;
          console.log(res);
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
              summary: 'Thông báo',
              detail: `Đã có lỗi !`,
            });
          }
        },
      });
  }


}

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SurveyDetailComponent } from './survey-detail.component';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { FormsModule, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerModule } from 'ngx-spinner';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

const routes: Routes = [
  {
    path: '',
    component: SurveyDetailComponent,
  },
];

@NgModule({
  declarations: [
    SurveyDetailComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(routes),
    ToastModule,
    ConfirmDialogModule,
    ReactiveFormsModule,
    NgbAlertModule,
    NgxSpinnerModule,
  ],
  providers: [MessageService, ConfirmationService, FormBuilder]
})
export class SurveyDetailModule { }

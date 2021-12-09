import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from './main.component';
import { TopbarComponent } from './shared/topbar/topbar.component';
import { FooterComponent } from './shared/footer/footer.component';
import { SidebarComponent } from './shared/sidebar/sidebar.component';


const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: '',
        loadChildren: () =>
          import('./survey/survey.module').then((m) => m.SurveyModule),
      },

      {
        path: 'category',
        loadChildren: () =>
          import('./category/category.module').then((m) => m.CategoryModule),
      },
      {
        path: 'survey',
        loadChildren: () =>
          import('./survey/survey.module').then((m) => m.SurveyModule),
      },
      {
        path: 'survey-detail/:surveyId',
        loadChildren: () =>
          import('./survey-detail/survey-detail.module').then((m) => m.SurveyDetailModule),
      },
      {
        path: 'report/:surveyId',
        loadChildren: () =>
          import('./report/report.module').then((m) => m.ReportModule),
      },
      {
        path: 'survey/:surveyId/question',
        loadChildren: () =>
          import('./question/question.module').then((m) => m.QuestionModule),
      },
      {
        path: 'survey/:surveyId/question/:questionId/answer',
        loadChildren: () =>
          import('./answer/answer.module').then((m) => m.AnswerModule),
      }
    ],
  },
];
@NgModule({
  declarations: [
    MainComponent,
    TopbarComponent,
    SidebarComponent,
    FooterComponent,
  ],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class MainModule { }

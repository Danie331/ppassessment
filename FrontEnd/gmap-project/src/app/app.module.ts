import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { GmapContainerComponent } from './gmap-container/gmap-container.component';
import { RouterModule } from '@angular/router';
import { GMapModule } from 'primeng/gmap';
import { SplitButtonModule } from 'primeng/splitbutton';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DropdownModule } from 'primeng/dropdown';
import { HttpClientModule } from '@angular/common/http';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';

@NgModule({
  declarations: [
    GmapContainerComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    BrowserAnimationsModule,
    DropdownModule,
    GMapModule,
    HttpClientModule,
    SplitButtonModule,
    MessagesModule,
    MessageModule,
    RouterModule.forRoot([
      { path: '', component: GmapContainerComponent }
    ]),
  ],
  providers: [],
  bootstrap: [GmapContainerComponent]
})
export class AppModule { }

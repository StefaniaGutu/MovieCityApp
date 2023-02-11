import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-basic-card',
  templateUrl: './basic-card.component.html',
  styleUrls: ['./basic-card.component.scss']
})
export class BasicCardComponent implements OnInit {
  @Input() image: string = '';
  @Input() title: string = '';
  @Input() id: Guid = Guid.createEmpty();
  @Input() isMovie: boolean = true;

  @Output() onClickCard = new EventEmitter<Guid>();

  constructor() { }

  ngOnInit(): void {
  }

  onClick(id: Guid){
      this.onClickCard.emit(id);
  }
}

import { Component, Input, OnInit } from '@angular/core';
import { MovieWithDetailsDto } from 'src/app/interfaces/movie-with-details-dto';

@Component({
  selector: 'app-movie-list',
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.scss']
})
export class MovieListComponent implements OnInit {
  @Input() moviesAndSeriesList: MovieWithDetailsDto[] = [];
  @Input() title: string = '';
  @Input() showNoResults: boolean = false;
  
  constructor() { }

  ngOnInit(): void {
  }

}

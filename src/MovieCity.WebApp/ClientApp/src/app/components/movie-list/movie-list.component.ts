import { Component, Input, OnInit } from '@angular/core';
import { MovieWithDetailsDto } from 'src/app/interfaces/movie-with-details-dto';
import { MovieDetailsComponent } from 'src/app/pages/movie-details/movie-details.component';

@Component({
  selector: 'app-movie-list',
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.scss']
})
export class MovieListComponent implements OnInit {
  @Input() moviesAndSeriesList: MovieWithDetailsDto[] = [];
  @Input() title: string = '';
  
  constructor() { }

  ngOnInit(): void {
  }

}

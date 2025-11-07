import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-error-page',
  templateUrl: './error-page.html',
  standalone: true,
  imports: [CommonModule, RouterLink]
})
export class ErrorPageComponent implements OnInit {
  
  private route = inject(ActivatedRoute);

  statusCode: number = 404;
  message: string = 'Page Not Found';

  ngOnInit(): void {

    this.route.url.subscribe(urlSegments => {

      if (urlSegments.length > 0) {
        const firstSegment = urlSegments[0].path;
        if (firstSegment === '404') {
            this.statusCode = 404;
            this.message = 'The requested resource was not found.';
        } else if (firstSegment === '500') {
            this.statusCode = 500;
            this.message = 'Internal Server Error. Please try again later.';
        }
      }
    });
  }
}
import { HttpErrorResponse } from "@angular/common/http";

export function toErrorMessage(err: HttpErrorResponse): string {
  const problem = err.error;

  if (problem?.errors) {
    return Object.values(problem.errors as Record<string, string[]>).flat().join(' ');
  }

  return problem?.detail ?? problem?.title ?? 'Something went wrong. Please try again.';
}
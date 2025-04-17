function getErrorMessages(errors: any) {
    if (errors == null) {
        return "Unknown error occurred";
    }
  if (errors && typeof errors === 'object') {
    return Object.values(errors)
      .flat()
      .join(", ");
  }

  if (Array.isArray(errors)) {
    if (errors.length === 0) {
      return "Unknown error occurred";
    }

    let errorMessage = "";
    for (const error of errors) {
      if (typeof error === 'object' && error !== null) {
        errorMessage += Object.values(error).join(", ")
      } else {
        errorMessage += String(error)
      }
    }
    return errorMessage;
  }

  return "Unknown error occurred";
}

export { getErrorMessages };
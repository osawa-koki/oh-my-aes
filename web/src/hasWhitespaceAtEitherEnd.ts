
function hasWhitespaceAtEitherEnd(str: string): boolean {
  if (/\s/.test(str.charAt(0)) || /\s/.test(str.charAt(str.length - 1))) {
    return true;
  }
  return false;
}

export default hasWhitespaceAtEitherEnd;

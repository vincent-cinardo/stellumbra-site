export function initWebGL(canvas) {
    const gl = canvas.getContext('webgl');
    if (!gl) {
        console.error('WebGL not supported');
        return;
    }

    gl.clearColor(0.0, 0.6, 0.8, 1.0);
    gl.clear(gl.COLOR_BUFFER_BIT);
    
}
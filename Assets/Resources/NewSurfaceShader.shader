Shader "Custom/NewSurfaceShader"
{
    properties
    {
        _Density("Density", Range(2, 100)) = 30
        _Alpha("Alpha", Range(0, 1)) = 0
    }

    SubShader
    {
        Pass
        {
            GLSLPROGRAM

            uniform float _Density;
            uniform float _Alpha;

            #ifdef VERTEX

            out vec4 texUvCoords; 

            void main()
            {
                texUvCoords = gl_MultiTexCoord0 * _Density;

                gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
            }

            #endif

            #ifdef FRAGMENT

            in vec4 texUvCoords;

            void main()
            {
                vec4 c = floor(texUvCoords) / 2;
                float f = fract(c.x + c.y) * 2;
                vec4 checker = vec4(f+_Alpha, 0, 0, 1);
                gl_FragColor = checker;
            }

            #endif

            ENDGLSL
        }
    }
}

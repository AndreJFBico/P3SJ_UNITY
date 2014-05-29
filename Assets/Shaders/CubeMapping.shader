Shader "Custom/CubeMapping" {
   Properties {
      _Cube("Reflection Map", Cube) = "" {}
	  _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
      _SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
	  _Shininess ("Shininess", Range(1.0, 20.0)) = 5.0
   }
   SubShader {
      Pass {
		 Tags { "LightMode" = "ForwardBase" }   
         GLSLPROGRAM

         uniform samplerCube _Cube;   
         uniform vec4 _Color; 
         uniform vec4 _SpecColor; 
         uniform float _Shininess;

         uniform vec3 _WorldSpaceCameraPos; 
         uniform mat4 _Object2World;
         uniform mat4 _World2Object;
         uniform vec4 _WorldSpaceLightPos0;
         uniform vec4 _LightColor0;
 
         varying vec3 normalDirection;
         varying vec3 viewDirection;

         varying vec4 position;
         varying float attenuation;
         varying vec3 H;
         varying vec3 L;
         varying mat3 localSurface2World;
 
         #ifdef VERTEX
 
         void main()
         {            
            mat4 modelMatrix = _Object2World;
            mat4 modelMatrixInverse = _World2Object;
 
            normalDirection = normalize(vec3(
               vec4(gl_Normal, 0.0) * modelMatrixInverse));
            viewDirection = vec3(modelMatrix * gl_Vertex 
               - vec4(_WorldSpaceCameraPos, 1.0));

			   position = modelMatrix *gl_Vertex;
		
            if (0.0 == _WorldSpaceLightPos0.w)
            {
               attenuation = 1.0; // no attenuation
               L = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               vec3 vertexToLightSource = 
                  vec3(_WorldSpaceLightPos0 - position);

               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance;

               L = normalize(vertexToLightSource);
            }

			   vec3 E = normalize(-position);
			   H = normalize(L + E);
 
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
 
         #ifdef FRAGMENT
 
         void main()
         {
   			vec3 ambientLight = vec3(gl_LightModel.ambient) * vec3(_Color);
    
   			float NdotL = clamp(dot(normalDirection,L),0.0,1.0);
   			vec3 diffuseLight = vec3(_LightColor0) * vec3(_Color) * NdotL;

   			vec3 specularLight = vec3(0.0);
   		
   			if(NdotL > 0.0){
   				vec3 R = reflect(-L,normalDirection);
   				float NdotH = clamp(dot(R,H),0.0,1.0);
   				float Blinn = pow(NdotH, _Shininess);
   				specularLight = vec3(_LightColor0) * vec3(_SpecColor) * Blinn;
   			}

            vec3 reflectedDirection = 
               reflect(viewDirection, normalize(normalDirection));

            gl_FragColor = textureCube(_Cube, reflectedDirection) * vec4(ambientLight
               + (diffuseLight + specularLight)*attenuation, 1.0);
         }
 
         #endif
 
         ENDGLSL
      }
	  Pass {
		 Tags { "LightMode" = "ForwardAdd" } 
            //pass for additional light sources
		 Blend One One //additive blending
		    
         GLSLPROGRAM

         uniform samplerCube _Cube;   
         uniform vec4 _Color; 
         uniform vec4 _SpecColor; 
         uniform float _Shininess;

         uniform vec3 _WorldSpaceCameraPos; 
         uniform mat4 _Object2World;
         uniform mat4 _World2Object;
         uniform vec4 _WorldSpaceLightPos0;
         uniform vec4 _LightColor0;
 
         varying vec3 normalDirection;
         varying vec3 viewDirection;

         varying vec4 position;
         varying float attenuation;
         varying vec3 H;
         varying vec3 L;
         varying mat3 localSurface2World;
 
         #ifdef VERTEX
 
         void main()
         {            
            mat4 modelMatrix = _Object2World;
            mat4 modelMatrixInverse = _World2Object;
 
            normalDirection = normalize(vec3(
               vec4(gl_Normal, 0.0) * modelMatrixInverse));
            viewDirection = vec3(modelMatrix * gl_Vertex 
               - vec4(_WorldSpaceCameraPos, 1.0));

         position = modelMatrix *gl_Vertex;
      
            if (0.0 == _WorldSpaceLightPos0.w)
            {
               attenuation = 1.0; // no attenuation
               L = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               vec3 vertexToLightSource = 
                  vec3(_WorldSpaceLightPos0 - position);

               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance;

               L = normalize(vertexToLightSource);
            }

         vec3 E = normalize(-position);
         H = normalize(L + E);

 
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
 
         #ifdef FRAGMENT
 
         void main()
         {
    
            float NdotL = clamp(dot(normalDirection,L),0.0,1.0);
            vec3 diffuseLight = vec3(_LightColor0) * vec3(_Color) * NdotL;

            vec3 specularLight = vec3(0.0);
         
            if(NdotL > 0.0){
               vec3 R = reflect(-L,normalDirection);
               float NdotH = clamp(dot(R,H),0.0,1.0);
               float Blinn = pow(NdotH, _Shininess);
               specularLight = vec3(_LightColor0) * vec3(_SpecColor) * Blinn;
            }

            vec3 reflectedDirection = 
               reflect(viewDirection, normalize(normalDirection));

            gl_FragColor = textureCube(_Cube, reflectedDirection) * vec4((diffuseLight + specularLight)*attenuation, 1.0);
         }
 
         #endif
 
         ENDGLSL
      }
   }
}
